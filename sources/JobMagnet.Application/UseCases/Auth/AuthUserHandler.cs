using JobMagnet.Application.Exceptions;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Application.UseCases.Auth.Ports;
using JobMagnet.Domain.Aggregates;
using Microsoft.Extensions.Options;

namespace JobMagnet.Application.UseCases.Auth;

public class AuthUserHandler(IUserManagerAdapter userManagerAdapter, IOptions<AdminUserOptions> options)
    : IAuthUserHandler
{
    public async Task<UserToken> RegisterAsync(UserModelCredentials userModelCredentials)
    {
        if (userModelCredentials == null)
            throw new ArgumentNullException(nameof(userModelCredentials));

        if (string.IsNullOrWhiteSpace(userModelCredentials.Email) ||
            string.IsNullOrWhiteSpace(userModelCredentials.Password))
            throw new ArgumentException("Password and email are required.");
        
        var token = await userManagerAdapter.RegisterAsync(userModelCredentials);
        
        return token;
    }
    
    public async Task<UserToken> LoginAsync(UserModelCredentials userModelCredentials)
    {
        if (string.IsNullOrWhiteSpace(userModelCredentials.Email) || string.IsNullOrWhiteSpace(userModelCredentials.Password))
            throw new ArgumentException("The email and password cannot be null, empty, or contain only spaces.");
        
        var token = await userManagerAdapter.LoginAsync(userModelCredentials);
        return false ? null : token;
    }
    
    public async Task<UserToken> CreateAdminUserAsync(CancellationToken cancellationToken)
    {
        var adminUserOptions = options.Value;
        try
        {
            var result = await userManagerAdapter.CreateAdminUserAsync(adminUserOptions, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            throw new AdminUserAlreadyExistsException(
                $"The administrator user with the email '{adminUserOptions.Email}' already exists.",
                ex
            );
        }
    }

}