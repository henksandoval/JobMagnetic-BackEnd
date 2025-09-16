using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Domain.Aggregates;
using JobMagnet.Domain.Aggregates.Auth.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Application.UseCases.Auth.Ports;

public interface IUserManage
{
    Task<UserToken> RegisterAsync(UserModelCredentials userModelCredentials, CancellationToken cancellationToken);
    Task<UserToken> LoginAsync(UserModelCredentials userModelCredentials, CancellationToken cancellationToken);
    Task<bool> EmailExistAsync(string email);    
    Task<UserToken> RefreshTokenAsync(RefreshToken  request);
    Task<UserToken> CreateAdminUserAsync(AdminUserOptions adminUserOptions,  CancellationToken cancellationToken);
}