using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Domain.Aggregates.Auth.Entities;

namespace JobMagnet.Application.UseCases.Auth.Interface;

public interface IAuthUserHandler
{
    Task<UserTokenDto> RegisterAsync(UserModelCredentialsDto userModelCredentialsDto, CancellationToken cancellationToken);
    Task<UserTokenDto> LoginAsync(UserModelCredentialsDto userModelCredentialsDto, CancellationToken cancellationToken);
    Task<UserTokenDto> RefreshTokenAsync(RefreshTokenDto  refreshTokenDto,  CancellationToken cancellationToken);
    Task<UserTokenDto> CreateAdminUserAsync(CancellationToken cancellationToken);
}