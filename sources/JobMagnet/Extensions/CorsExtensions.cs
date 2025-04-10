using JobMagnet.Extensions.ConfigSections;

namespace JobMagnet.Extensions;

internal static class CorsExtensions
{
    internal static IServiceCollection AddCorsPolicies(this IServiceCollection service, IConfiguration configuration)
    {
        var clientSettings = configuration.GetSection("ClientSettings").Get<ClientSettings>();

        if (clientSettings == null || string.IsNullOrWhiteSpace(clientSettings.Url))
            throw new InvalidOperationException("ClientSettings:Url is not set in the configuration.");

        service.AddCors(options =>
        {
            options.AddPolicy("DefaultCorsPolicy", policy =>
            {
                policy
                    .WithOrigins(clientSettings.Url)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

        });

        return service;
    }
}