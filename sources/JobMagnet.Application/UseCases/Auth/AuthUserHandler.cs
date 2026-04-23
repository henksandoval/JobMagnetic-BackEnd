using JobMagnet.Application.Exceptions;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.DTO.ForgotPassword;
using JobMagnet.Application.UseCases.Auth.DTO.GoogleLogin;
using JobMagnet.Application.UseCases.Auth.DTO.Logout;
using JobMagnet.Application.UseCases.Auth.DTO.PasswordResetConfirm;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Application.UseCases.Auth.Ports;
using JobMagnet.Domain.Aggregates;
using JobMagnet.Domain.Exceptions;
using Microsoft.Extensions.Options;

namespace JobMagnet.Application.UseCases.Auth;

public class AuthUserHandler(IUserManage userManager, IOptions<AdminUserOptions> options)
    : IAuthUserHandler
{
    public async Task<UserTokenDto> RegisterAsync(UserModelCredentialsDto userModelCredentialsDto,
        CancellationToken cancellationToken)
    {
        if (userModelCredentialsDto == null)
            throw new ArgumentNullException(nameof(userModelCredentialsDto));

        if (string.IsNullOrWhiteSpace(userModelCredentialsDto.Email) ||
            string.IsNullOrWhiteSpace(userModelCredentialsDto.Password))
            throw new ArgumentException("Password and email are required.");

        if (await userManager.EmailExistAsync(userModelCredentialsDto.Email))
            throw new JobMagnetApplicationException("Email already exists.");

        var token = await userManager.RegisterAsync(userModelCredentialsDto, cancellationToken);

        return token;
    }

    public async Task<UserTokenDto> LoginAsync(UserModelCredentialsDto userModelCredentialsDto,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userModelCredentialsDto.Email) ||
            string.IsNullOrWhiteSpace(userModelCredentialsDto.Password))
            throw new ArgumentException("The email and password cannot be null or empty.");

        return await userManager.LoginAsync(userModelCredentialsDto, cancellationToken);
    }

    public async Task<GoogleTokenInfoDto> LoginGoogleAsync(GoogleLoginCommand loginCommand,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(loginCommand.IdToken))
            throw new ArgumentException("The Google IdToken cannot be null, empty, or contain only spaces.");

        var googleUser = await userManager.LoginGoogleAsync(loginCommand, cancellationToken);

        if (googleUser == null)
            throw new InvalidGoogleTokenException("The Google token is invalid or has expired.");

        if (string.IsNullOrWhiteSpace(googleUser.Email) || string.IsNullOrWhiteSpace(googleUser.DisplayName))
            throw new ArgumentException("The email and name returned by Google cannot be null or empty.");

        return googleUser;
    }

    public async Task<UserTokenDto> RefreshTokenAsync(RefreshTokenDto? refreshTokenDto,
        CancellationToken cancellationToken)
    {
        if (refreshTokenDto == null || string.IsNullOrWhiteSpace(refreshTokenDto.RefreshToken))
            return null!;

        return await userManager.RefreshTokenAsync(refreshTokenDto, cancellationToken);
    }

    public async Task<UserTokenDto> CreateAdminUserAsync(CancellationToken cancellationToken)
    {
        var adminUserOptions = options.Value;
        try
        {
            var result = await userManager.CreateAdminUserAsync(adminUserOptions, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            throw new AdminUserAlreadyExistsException(
                $"The administrator user with the email '{adminUserOptions.Email}' already exists.",
                ex
            );
        }
    }

    public async Task<bool> LogoutAsync(LogoutCommand command, CancellationToken cancellationToken)
    {
        return await userManager.LogoutAsync(command, cancellationToken);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        await userManager.GeneratePasswordResetTokenAsync(command.Email, cancellationToken);
    }

    public async Task ConfirmPasswordResetAsync(PasswordResetConfirmDto dto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Token) ||
            string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("All fields are required.");

        await userManager.ConfirmPasswordResetAsync(dto, cancellationToken);
    }
}