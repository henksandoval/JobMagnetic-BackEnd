using JobMagnet.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Domain.Extensions;

public static class DomainExtensions
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<IProfileIdentifierNameGenerator, ProfileIdentifierNameGenerator>();

        return services;
    }
}