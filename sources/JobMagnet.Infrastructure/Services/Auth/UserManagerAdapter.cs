using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Ports;
using JobMagnet.Domain.Aggregates;
using JobMagnet.Infrastructure.ExternalServices.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JobMagnet.Infrastructure.Services.Auth;

public class UserManagerAdapter(UserManager<ExternalServices.Identity.Entities.ApplicationIdentityUser> userManager, IConfiguration configuration) : IUserManagerAdapter
{
    private readonly UserManager<ExternalServices.Identity.Entities.ApplicationIdentityUser> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;

    public async Task<UserToken> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return await BuildToken(loginDto);
        }
        return null!;
    }

    public async Task<UserToken> CreateAdminUserAsync (AdminUser adminUser, CancellationToken cancellationToken)
    {
        var applicationIdentityUser = new ApplicationIdentityUser
        {
            UserName = adminUser.Email,
            Email = adminUser.Email,
        };
        
        var result = await _userManager.CreateAsync(applicationIdentityUser, adminUser.Password);
        if (!result.Succeeded)
            throw new Exception("The administrator user could not be created: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        
        var loginDto = new LoginDto { Email = applicationIdentityUser.Email, Password = adminUser.Password };
        return await BuildToken(loginDto);
    }

    private async Task<UserToken> BuildToken(LoginDto loginDto)
    {
        var claims = new List<Claim>
        { 
            new Claim(ClaimTypes.Name, loginDto.Email),
            new Claim(ClaimTypes.Email, loginDto.Email)
        };
        var userIdentity = await _userManager.FindByNameAsync(loginDto.Email);

        if (userIdentity?.Id != null) 
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userIdentity.Id.ToString()));
        
        var clamsDB = await _userManager.GetClaimsAsync(userIdentity);
        claims.AddRange(clamsDB);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var expiration = DateTime.UtcNow.AddYears(1);
        
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