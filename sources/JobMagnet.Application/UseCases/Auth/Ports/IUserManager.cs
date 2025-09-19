using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Domain.Aggregates;
using JobMagnet.Domain.Aggregates.Auth.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Application.UseCases.Auth.Ports;

public interface IUserManage
{
    Task<UserTokenDto> RegisterAsync(UserModelCredentialsDto userModelCredentialsDto, CancellationToken cancellationToken);
    Task<UserTokenDto> LoginAsync(UserModelCredentialsDto userModelCredentialsDto, CancellationToken cancellationToken);
    Task<bool> EmailExistAsync(string email);    
    Task<UserTokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto,  CancellationToken cancellationToken);
    Task<UserTokenDto> CreateAdminUserAsync(AdminUserOptions adminUserOptions,  CancellationToken cancellationToken);
}