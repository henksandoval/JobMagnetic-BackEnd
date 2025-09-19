using JobMagnet.Application.Exceptions;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Application.UseCases.Auth.Ports;
using JobMagnet.Domain.Aggregates;
using JobMagnet.Domain.Aggregates.Auth.Entities;
using Microsoft.Extensions.Options;

namespace JobMagnet.Application.UseCases.Auth;

public class AuthUserHandler(IUserManage userManager, IOptions<AdminUserOptions> options)
    : IAuthUserHandler
{
    public async Task<UserToken> RegisterAsync(UserModelCredentials userModelCredentials, CancellationToken cancellationToken)
    {
        if (userModelCredentials == null)
            throw new ArgumentNullException(nameof(userModelCredentials));

        if (string.IsNullOrWhiteSpace(userModelCredentials.Email) ||
            string.IsNullOrWhiteSpace(userModelCredentials.Password))
            throw new ArgumentException("Password and email are required.");

        if (await userManager.EmailExistAsync(userModelCredentials.Email))
            throw new JobMagnetApplicationException("Email already exists.");
        
        var token = await userManager.RegisterAsync(userModelCredentials, cancellationToken);
        
        return token;
    }
    
    public async Task<UserToken> LoginAsync(UserModelCredentials userModelCredentials, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userModelCredentials.Email) || string.IsNullOrWhiteSpace(userModelCredentials.Password))
            throw new ArgumentException("The email and password cannot be null, empty, or contain only spaces.");
        
        var token = await userManager.LoginAsync(userModelCredentials, cancellationToken);
        return false ? null : token;
    }

    public async Task<UserToken> RefreshTokenAsync(RefreshToken request)
    {
        if (string.IsNullOrWhiteSpace(request.ToString()))
        {
            return null;
        }
        return await userManager.RefreshTokenAsync(request);
    }

    public async Task<UserToken> CreateAdminUserAsync(CancellationToken cancellationToken)
    {
        var adminUserOptions = options.Value;
        try
        {
            var result = await userManager.CreateAdminUserAsync(adminUserOptions, cancellationToken);
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