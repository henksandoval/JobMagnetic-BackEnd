using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Domain.Aggregates;

namespace JobMagnet.Application.UseCases.Auth.Ports;

public interface IUserManagerAdapter
{
    Task<UserToken> LoginAsync(LoginDto loginDto);
    Task<UserToken> CreateAdminUserAsync(AdminUser adminUser,  CancellationToken cancellationToken);
}