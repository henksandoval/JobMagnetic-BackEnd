using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.DTO.ForgotPassword;
using JobMagnet.Application.UseCases.Auth.DTO.Logout;

namespace JobMagnet.Application.UseCases.Auth.Interface;

public interface IAuthUserHandler
{
    Task<UserTokenDto> RegisterAsync(UserModelCredentialsDto userModelCredentialsDto, CancellationToken cancellationToken);
    Task<UserTokenDto> LoginAsync(UserModelCredentialsDto userModelCredentialsDto, CancellationToken cancellationToken);
    Task<UserTokenDto> RefreshTokenAsync(RefreshTokenDto  refreshTokenDto,  CancellationToken cancellationToken);
    Task<UserTokenDto> CreateAdminUserAsync(CancellationToken cancellationToken);
    Task<bool> LogoutAsync(LogoutCommand command, CancellationToken cancellationToken);
    Task ForgotPasswordAsync(ForgotPasswordCommand command, CancellationToken cancellationToken);
}