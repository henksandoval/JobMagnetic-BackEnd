using JobMagnet.Extensions.SettingSections;

namespace JobMagnet.Extensions;

internal static class LoadSettingExtensions
{
    internal static IServiceCollection AddSettingSections(this IServiceCollection service, IConfiguration configuration)
    {
        return service.AddClientSettings(configuration);
    }

    private static IServiceCollection AddClientSettings(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddOptionsWithValidateOnStart<ClientSettings>()
            .Bind(configuration.GetSection("ClientSettings"));

        return service;
    }
}