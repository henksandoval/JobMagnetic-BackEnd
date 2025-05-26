using JobMagnet.Extensions.SettingSections;

namespace JobMagnet.Extensions;

internal static class CorsExtensions
{
    internal static IServiceCollection AddCorsPolicies(this IServiceCollection service, IConfiguration configuration)
    {
        var allowOrigins = configuration.GetSection("AllowOrigins").Get<AllowOrigins>();

        if (allowOrigins == null || allowOrigins.Origins.Length == 0)
            throw new InvalidOperationException("AllowOrigins:Origins is not set in the configuration.");

        service.AddCors(options =>
        {
            options.AddPolicy("DefaultCorsPolicy", policy =>
            {
                policy
                    .WithOrigins(allowOrigins.Origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return service;
    }
}