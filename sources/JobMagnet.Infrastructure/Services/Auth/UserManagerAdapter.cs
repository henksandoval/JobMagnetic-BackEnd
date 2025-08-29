using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Ports;
using JobMagnet.Application.UseCases.Auth.Ports.EmailDTO;
using JobMagnet.Domain.Aggregates;
using JobMagnet.Infrastructure.Exceptions;
using JobMagnet.Infrastructure.ExternalServices.Identity.Entities;
using JobMagnet.Infrastructure.Services.EmailService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JobMagnet.Infrastructure.Services.Auth;

public class UserManagerAdapter(UserManager<ExternalServices.Identity.Entities.ApplicationIdentityUser> userManager, IConfiguration configuration, IEmailService emailService) : IUserManagerAdapter
{
    private readonly UserManager<ExternalServices.Identity.Entities.ApplicationIdentityUser> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly IEmailService _emailService = emailService;
    
    public async Task<UserToken> RegisterAsync(UserModelCredentials  userModelCredentials)
    {
        
        var userExists = await _userManager.FindByEmailAsync(userModelCredentials.Email);
        if (userExists != null)
        {
            throw new EmailAlreadyTakenAdapterException($"The email'{userModelCredentials.Email}' already in use.");
        }
        
        var user = new ApplicationIdentityUser
        {
            UserName = userModelCredentials.Email, 
            Email = userModelCredentials.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        
        var result = await _userManager.CreateAsync(user, userModelCredentials.Password);
        
        if (result.Succeeded)
        {
            
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = System.Net.WebUtility.UrlEncode(confirmationToken);
            
            var confirmationUrlBase = _configuration["ClientApp:ConfirmationUrl"];
            var confirmationLink = $"{confirmationUrlBase}?email={user.Email}&token={encodedToken}";
            
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
            
            await  _emailService.SendEmailAsync(mailCommand);
            return await BuildToken(user);
        }
        else
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"No se pudo crear el usuario: {errors}");
        }
    }

    public async Task<UserToken> LoginAsync(UserModelCredentials userModelCredentials)
    {
        var user = await _userManager.FindByEmailAsync(userModelCredentials.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, userModelCredentials.Password))
        {
            return await BuildToken(user);
        }
        
        throw new InvalidCredentialsAdapterException("Incorrect email or password.");
    }

    public async Task<UserToken> CreateAdminUserAsync (AdminUserOptions adminUserOptions, CancellationToken cancellationToken)
    {
        var applicationIdentityUser = new ApplicationIdentityUser
        {
            UserName = adminUserOptions.Email,
            Email = adminUserOptions.Email,
        };
        
        var result = await _userManager.CreateAsync(applicationIdentityUser, adminUserOptions.Password);
        if (result.Errors.Any(e => e.Code is "DuplicateUserName" or "DuplicateEmail"))
        {
            throw new EmailAlreadyTakenAdapterException($"The email'{adminUserOptions.Email}' already in use.");
        }
        
        var loginDto = new UserModelCredentials { Email = applicationIdentityUser.Email, Password = adminUserOptions.Password };
        return await BuildToken(applicationIdentityUser);
    }

    private async Task<UserToken> BuildToken(ApplicationIdentityUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        
        var userClaims = await _userManager.GetClaimsAsync(user);
        
        var claims = new List<Claim>
        { 
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Email)
        };
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        claims.AddRange(userClaims);
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var expiration = DateTime.UtcNow.AddHours(1);
        
        var securityToken = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );
        
        return new UserToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expiration = expiration
        };
    }
}