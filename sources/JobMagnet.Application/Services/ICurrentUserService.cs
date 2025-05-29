namespace JobMagnet.Application.Services;

public interface ICurrentUserService
{
    Guid? GetCurrentUserId();
}