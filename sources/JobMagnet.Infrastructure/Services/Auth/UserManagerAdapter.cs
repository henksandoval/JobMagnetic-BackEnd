using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.DTO.Logout;
using JobMagnet.Application.UseCases.Auth.Ports;
using JobMagnet.Application.UseCases.Auth.Ports.EmailDTO;
using JobMagnet.Domain.Aggregates;
using JobMagnet.Domain.Aggregates.Auth.Entities;
using JobMagnet.Domain.Aggregates.Auth.ValueObjects;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Infrastructure.Exceptions;
using JobMagnet.Infrastructure.ExternalServices.Identity.Entities;
using JobMagnet.Infrastructure.Services.EmailService.Interfaces;
using JobMagnet.Shared.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JobMagnet.Infrastructure.Services.Auth;

public class UserManagerAdapter(
    UserManager<ApplicationIdentityUser> userManager,
    IConfiguration configuration,
    IEmailService emailService,
    IUnitOfWork unitOfWork,
    IGuidGenerator guidGenerator) : IUserManage
{
    private readonly IGuidGenerator _guidGenerator = guidGenerator;
    
    public async Task<UserTokenDto> RegisterAsync(UserModelCredentialsDto  userModelCredentialsDto, CancellationToken cancellationToken)
    {
        var appUser = new ApplicationIdentityUser
        {
            UserName = userModelCredentialsDto.Email, 
            Email = userModelCredentialsDto.Email
        };
        
        var identityResult  = await userManager.CreateAsync(appUser, userModelCredentialsDto.Password);
        
        if (!identityResult.Succeeded)
        {
            var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Error de Identity: {errors}");
        }
        
        try
        {
                
            var domainUserId  = _guidGenerator.NewGuid();
            var domainUser = User.AddUser(new UserId(domainUserId ), appUser.Email, userModelCredentialsDto.DisplayName, null, appUser.Id);
            appUser.User = domainUser;
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            
            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var encodedToken = System.Net.WebUtility.UrlEncode(confirmationToken);
            
            var confirmationUrlBase = configuration["ClientApp:ConfirmationUrl"];
            var confirmationLink = $"{confirmationUrlBase}?email={appUser.Email}&token={encodedToken}";
            
            var mailCommand = new MailCommand
            {
                ToEmail = userModelCredentialsDto.Email,
                Subject = "Confirma tu cuenta en JobMagnet",
                Body =$"""
                           <h1>¡Bienvenido a JobMagnet!</h1>
                           <p>Gracias por registrarte. Por favor, confirma tu cuenta haciendo clic en el siguiente enlace:</p>
                           <p><a href="{confirmationLink}">Confirmar mi cuenta</a></p>
                           <p>Saludos,<br>El equipo de JobMagnet</p>
                       """
            };
            
            await  emailService.SendEmailAsync(mailCommand);
            
            return await GenerateTokensAsync(appUser, cancellationToken);
        }
        catch (Exception exception)
        {
            await userManager.DeleteAsync(appUser);
            throw;
        }
    }

    public async Task<UserTokenDto> LoginAsync(UserModelCredentialsDto userModelCredentialsDto, CancellationToken cancellationToken)
    {
        var identityUser = await userManager.Users
            .Include(u => u.User) 
            .SingleOrDefaultAsync(u => u.Email == userModelCredentialsDto.Email, cancellationToken);
        
        if (identityUser == null || !await userManager.CheckPasswordAsync(identityUser, userModelCredentialsDto.Password))
            throw new InvalidCredentialsAdapterException("Incorrect email or password.");

        if (!await userManager.IsEmailConfirmedAsync(identityUser))
            throw new InvalidOperationException("Email not confirmed.");

        return await GenerateTokensAsync(identityUser, cancellationToken);
    }


    public async  Task<UserTokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto,  CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .Include(u => u.User)
            .ThenInclude(domainUser => domainUser.RefreshTokens)
            .SingleOrDefaultAsync(u => u.User.RefreshTokens.Any(rt => rt.Token == refreshTokenDto.RefreshToken), cancellationToken);

        if (user == null)
            return null!;
        
        var tokenToValidate = user.User.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshTokenDto.RefreshToken);
        
        if (tokenToValidate is not { IsActive: true })
            return null!;
        
        foreach (var activeToken in user.User.RefreshTokens.Where(rt => rt.IsActive).ToList())
        {
            user.User.RevokeRefreshToken(activeToken.Token);
        }
        
        var newTokens = await GenerateTokensAsync(user, cancellationToken);

        return newTokens;
    }
    
    public async Task<bool> LogoutAsync(LogoutCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .Include(u => u.User)
            .ThenInclude(domainUser => domainUser.RefreshTokens)
            .SingleOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
    
        var tokenToRevoke = user?.User.RefreshTokens.FirstOrDefault(rt => rt.Token == command.RefreshToken);
    
        if (tokenToRevoke is not { IsActive: true }) return false;
        
        user?.User.RevokeRefreshToken(command.RefreshToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
    
    public async Task<UserTokenDto> CreateAdminUserAsync (AdminUserOptions adminUserOptions, CancellationToken cancellationToken)
    {
        var applicationIdentityUser = new ApplicationIdentityUser
        {
            UserName = adminUserOptions.Email,
            Email = adminUserOptions.Email,
        };
        
        var result = await userManager.CreateAsync(applicationIdentityUser, adminUserOptions.Password);
        if (result.Errors.Any(e => e.Code is "DuplicateUserName" or "DuplicateEmail"))
        {
            throw new EmailAlreadyTakenAdapterException($"The email'{adminUserOptions.Email}' already in use.");
        }

        return await GenerateTokensAsync(applicationIdentityUser, cancellationToken);
    }

    private async Task<UserTokenDto> GenerateTokensAsync(ApplicationIdentityUser user,  CancellationToken cancellationToken)
    {
        var userRoles = await userManager.GetRolesAsync(user);
        
        var claims = new List<Claim>
        { 
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Email)
        };
        
        var applicationIdentityUser = await userManager.FindByEmailAsync(user.Email);
        if (applicationIdentityUser != null)
        {
            var userClaims = await userManager.GetClaimsAsync(applicationIdentityUser);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            claims.AddRange(userClaims);
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"] ?? string.Empty));
        var creeds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = Convert.ToDouble(configuration["JWT:TokenValidityInMinutes"] ?? "15");
        var jwtExpiration = DateTime.UtcNow.AddMinutes(expiration);
        var dtoExpiration = DateTime.Now.AddMinutes(expiration); 
        
        var securityToken = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: jwtExpiration,
            signingCredentials: creeds
        );
        
        var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        
        var refreshTokenResponse = GenerateRefreshTokenString();
        var refreshTokenValidityInDays = Convert.ToInt32(configuration["JWT:RefreshTokenValidityInDays"] ?? "7");
        var refreshTokenValidity = TimeSpan.FromDays(refreshTokenValidityInDays);

        user.User.AddRefreshToken(
            _guidGenerator, 
            refreshTokenResponse, 
            refreshTokenValidity
        );
        
        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to update user with new refresh token: {errors}");
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new UserTokenDto()
        {
            AccessToken = accessToken ,
            ExpiresInSeconds = dtoExpiration,
            RefreshToken = refreshTokenResponse
        };
    }

    public async Task GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(email);
        
        if (user == null)
        {
            return; 
        }
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var encodedToken = System.Net.WebUtility.UrlEncode(token);
        var resetUrlBase = configuration["ClientApp:ResetPasswordUrl"];
        var resetLink = $"{resetUrlBase}?email={email}&token={encodedToken}";
        
        var mailCommand = new MailCommand
        {
            ToEmail = email,
            Subject = "Recuperación de contraseña - JobMagnet",
            Body = $"""
                        <h1>Recuperación de Contraseña</h1>
                        <p>Hemos recibido una solicitud para restablecer tu contraseña en JobMagnet.</p>
                        <p>Haz clic en el siguiente enlace para crear una nueva contraseña:</p>
                        <p><a href="{resetLink}">Restablecer mi contraseña</a></p>
                        <p>Si no solicitaste esto, puedes ignorar este correo.</p>
                        <p>Saludos,<br>El equipo de JobMagnet</p>
                    """
        };
        await emailService.SendEmailAsync(mailCommand);
    }
    
    private static string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    public async Task<bool> EmailExistAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user != null;
    }
}
