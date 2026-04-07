namespace JobMagnet.Host.Extensions;

public static class HttpResponseExtensions
{
    public static void AppendRefreshTokenCookie(this HttpResponse response, string refreshToken, int expirationDays = 7)
    {
        response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(expirationDays)
        });
    }
}