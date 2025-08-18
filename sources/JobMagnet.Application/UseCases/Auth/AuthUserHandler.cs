using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;

namespace JobMagnet.Application.UseCases.Auth;

public class AuthUserHandler(IUserManagerAdapter userManagerAdapter) : IAuthUserHandler
{
    public Task<UserToken> LoginAsync(LoginDto loginDto)
        => throw new NotImplementedException();
}