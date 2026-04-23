namespace JobMagnet.Application.UseCases.Auth.DTO.PasswordResetConfirm;

public record PasswordResetConfirmDto(string Email, string Token, string Password);