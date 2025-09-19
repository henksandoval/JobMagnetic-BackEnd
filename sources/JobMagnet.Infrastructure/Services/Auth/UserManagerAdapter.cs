using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JobMagnet.Application.UseCases.Auth.DTO;
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
    ICommandRepository<User> repository,
    IUnitOfWork unitOfWork,
    IGuidGenerator guidGenerator) : IUserManage
{
    private readonly IGuidGenerator _guidGenerator = guidGenerator;

    
    public async Task<UserToken> RegisterAsync(UserModelCredentials  userModelCredentials, CancellationToken cancellationToken)
    {
        var appUser = new ApplicationIdentityUser
        {
            UserName = userModelCredentials.Email, 
            Email = userModelCredentials.Email
        };
        
        var identityResult  = await userManager.CreateAsync(appUser, userModelCredentials.Password);
        
        if (!identityResult.Succeeded)
        {
            var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Error de Identity: {errors}");
        }
        
        try
        {
                
            var domainUserId  = _guidGenerator.NewGuid();
            var domainUser = User.AddUser(new UserId(domainUserId ), appUser.Email, null, appUser.Id);
            appUser.User = domainUser;
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            
            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var encodedToken = System.Net.WebUtility.UrlEncode(confirmationToken);
            
            var confirmationUrlBase = configuration["ClientApp:ConfirmationUrl"];
            var confirmationLink = $"{confirmationUrlBase}?email={appUser.Email}&token={encodedToken}";
            
            var mailCommand = new MailCommand
            {
                ToEmail = userModelCredentials.Email,
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
        catch (Exception)
        {
            await userManager.DeleteAsync(appUser);
            throw;
        }
    }

    public async Task<UserToken> LoginAsync(UserModelCredentials userModelCredentials, CancellationToken cancellationToken)
    {
        
        var identityUser = await userManager.Users
            .Include(u => u.User) 
            .SingleOrDefaultAsync(u => u.Email == userModelCredentials.Email, cancellationToken);
        
        if (identityUser == null || !await userManager.CheckPasswordAsync(identityUser, userModelCredentials.Password))
            throw new InvalidCredentialsAdapterException("Incorrect email or password.");

        if (!await userManager.IsEmailConfirmedAsync(identityUser))
            throw new InvalidOperationException("Email not confirmed.");

        return await GenerateTokensAsync(identityUser, cancellationToken);
    }

    public async Task<bool> EmailExistAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user != null;
    }

    public  Task<UserToken> RefreshTokenAsync(RefreshToken request)
    {
            throw new NotImplementedException();
    }

    public async Task<UserToken> CreateAdminUserAsync (AdminUserOptions adminUserOptions, CancellationToken cancellationToken)
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
        
        var loginDto = new UserModelCredentials { Email = applicationIdentityUser.Email, Password = adminUserOptions.Password };
        return await GenerateTokensAsync(applicationIdentityUser, cancellationToken);
    }

    private async Task<UserToken> GenerateTokensAsync(ApplicationIdentityUser user,  CancellationToken cancellationToken)
    {
        var userRoles = await userManager.GetRolesAsync(user);
        
        var userClaims = await userManager.GetClaimsAsync(user);
        
        var claims = new List<Claim>
        { 
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Email)
        };
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        claims.AddRange(userClaims);
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["JWT:TokenValidityInMinutes"] ?? "15"));
        
        var securityToken = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: creds
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
        
        return new UserToken()
        {
            Token = accessToken ,
            Expiration = expiration,
            RefreshToken = refreshTokenResponse
        };
    }
    private static string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}