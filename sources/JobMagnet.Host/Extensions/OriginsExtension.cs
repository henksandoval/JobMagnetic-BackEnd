using JobMagnet.Host.Extensions.SettingSections;

namespace JobMagnet.Host.Extensions;

internal static class OriginsExtension
{
    internal static IServiceCollection AddAllowOrigins(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddOptionsWithValidateOnStart<AllowOrigins>()
            .Bind(configuration.GetSection(AllowOrigins.Key));

        return service;
    }
}