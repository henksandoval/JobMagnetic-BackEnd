using JobMagnet.Application.UseCases.CvParser;
using JobMagnet.Domain.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services
            .AddLogging()
            .AddDomainDependencies()
            .AddTransient<ICvParserHandler, CvParserHandler>();
    }
}