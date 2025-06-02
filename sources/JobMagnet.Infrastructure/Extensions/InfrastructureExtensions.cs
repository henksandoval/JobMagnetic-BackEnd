using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Infrastructure.ExternalServices.CvParsers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddTransient<IRawCvParser, GeminiCvParser>()
            .AddPersistence()
            .AddGemini(configuration);
    }
}