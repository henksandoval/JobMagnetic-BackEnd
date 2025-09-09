using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Domain.Aggregates;
using JobMagnet.Domain.Aggregates.Auth.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Application.UseCases.Auth.Ports;

public interface IUserManagerAdapter
{
    Task<UserToken> RegisterAsync(UserModelCredentials userModelCredentials);
    Task<UserToken> LoginAsync(UserModelCredentials userModelCredentials);
    
    Task<UserToken> RefreshTokenAsync(RefreshToken  request);
    Task<UserToken> CreateAdminUserAsync(AdminUserOptions adminUserOptions,  CancellationToken cancellationToken);
}