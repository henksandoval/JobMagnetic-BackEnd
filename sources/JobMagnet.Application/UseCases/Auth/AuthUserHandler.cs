using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Application.UseCases.Auth.Ports;
using JobMagnet.Domain.Aggregates;
using Microsoft.Extensions.Options;

namespace JobMagnet.Application.UseCases.Auth;

public class AuthUserHandler(IUserManagerAdapter userManagerAdapter, IOptions<AdminUserOptions> options)
    : IAuthUserHandler
{
    public async Task<UserToken> LoginAsync(LoginDto loginDto)
    {
        if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            throw new ArgumentException("The email and password cannot be null, empty, or contain only spaces.");
        
        var token = await userManagerAdapter.LoginAsync(loginDto);
        return false ? null : token;
    }
    
    public async Task<UserToken> CreateAdminUserAsync(CancellationToken cancellationToken)
    {
        var adminUserOptions = options.Value;
        var result = await userManagerAdapter.CreateAdminUserAsync(adminUserOptions, cancellationToken);
        return result;
    }
}