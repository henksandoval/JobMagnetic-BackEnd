using JobMagnet.Application.UseCases.Auth.DTO;

namespace JobMagnet.Application.UseCases.Auth.Interface;

public interface IUserManagerAdapter
{
    Task<UserToken> LoginAsync(LoginDto loginDto);
}