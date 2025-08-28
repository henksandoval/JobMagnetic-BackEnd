using JobMagnet.Application.UseCases.Auth.DTO;

namespace JobMagnet.Application.UseCases.Auth.Interface;

public interface IAuthUserHandler
{
    Task<UserToken> LoginAsync(UserModelCredentials userModelCredentials);
    Task<UserToken> CreateAdminUserAsync(CancellationToken cancellationToken);
    Task<UserToken> RegisterAsync(UserModelCredentials userModelCredentials);
}