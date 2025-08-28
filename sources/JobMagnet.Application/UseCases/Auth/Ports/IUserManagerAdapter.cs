using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Domain.Aggregates;

namespace JobMagnet.Application.UseCases.Auth.Ports;

public interface IUserManagerAdapter
{
    Task<UserToken> LoginAsync(UserModelCredentials userModelCredentials);
    Task<UserToken> CreateAdminUserAsync(AdminUserOptions adminUserOptions,  CancellationToken cancellationToken);
    Task<UserToken> RegisterAsync(UserModelCredentials userModelCredentials);
}