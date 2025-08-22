using JobMagnet.Application.UseCases.Auth.DTO;

namespace JobMagnet.Application.UseCases.Auth.Interface;

public interface IAuthUserHandler
{
    Task<UserToken> LoginAsync(LoginDto loginDto);
    Task<UserToken> CreateAdminUserAsync(CancellationToken cancellationToken);
}