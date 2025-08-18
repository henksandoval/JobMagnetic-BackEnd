using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JobMagnet.Infrastructure.Services.Auth;

public class UserManagerAdapter(UserManager<IdentityUser> userManager, IConfiguration configuration) : IUserManagerAdapter
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;

    public async Task<UserToken> LoginAsync(LoginDto loginDto)
    {
        var user = new IdentityUser{UserName = loginDto.Email, Email = loginDto.Email};
        var result =  await _userManager.CreateAsync(user, loginDto.Password);

        if (result.Succeeded)
        {
            return await BuildToken(loginDto);
        }
        throw new Exception("Login failed");
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
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userIdentity.Id));
        
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