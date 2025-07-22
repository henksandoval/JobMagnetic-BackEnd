using JobMagnet.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Shared.Extensions;

public static class SharedExtensions
{
    public static IServiceCollection AddSharedDependencies(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddSingleton<IClock, Clock>()
            .AddSingleton<IGuidGenerator, GuidGenerator>();

        return services;
    }
}