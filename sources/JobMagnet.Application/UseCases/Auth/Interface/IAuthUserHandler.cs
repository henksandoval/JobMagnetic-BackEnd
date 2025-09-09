using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Domain.Aggregates.Auth.Entities;

namespace JobMagnet.Application.UseCases.Auth.Interface;

public interface IAuthUserHandler
{
    Task<UserToken> RegisterAsync(UserModelCredentials userModelCredentials);
    Task<UserToken> LoginAsync(UserModelCredentials userModelCredentials);
    Task<UserToken> RefreshTokenAsync(RefreshToken  request);
    Task<UserToken> CreateAdminUserAsync(CancellationToken cancellationToken);
}