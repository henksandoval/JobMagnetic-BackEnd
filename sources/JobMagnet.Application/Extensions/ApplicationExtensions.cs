using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        return services
            .AddLogging();
    }
}