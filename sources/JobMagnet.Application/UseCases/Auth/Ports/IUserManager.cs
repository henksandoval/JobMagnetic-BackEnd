using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.DTO.ForgotPassword;
using JobMagnet.Domain.Aggregates;

namespace JobMagnet.Application.UseCases.Auth.Ports;

public interface IUserManage
{
    Task<UserTokenDto> RegisterAsync(UserModelCredentialsDto userModelCredentialsDto, CancellationToken cancellationToken);
    Task<UserTokenDto> LoginAsync(UserModelCredentialsDto userModelCredentialsDto, CancellationToken cancellationToken);
    Task<bool> EmailExistAsync(string email);    
    Task<UserTokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto,  CancellationToken cancellationToken);
    Task<UserTokenDto> CreateAdminUserAsync(AdminUserOptions adminUserOptions,  CancellationToken cancellationToken);
    Task<bool> LogoutAsync(LogoutCommand command, CancellationToken cancellationToken);
    Task GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken);
}