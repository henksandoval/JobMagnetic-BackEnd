using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Infrastructure.Services.CvParsers;
using JobMagnet.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddSharedDependencies()
            .AddTransient<IRawCvParser, GeminiCvParser>()
            .AddPersistence()
            .AddGemini(configuration);
}