using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Infrastructure.Services.Auth;
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
            .AddTransient<IUserManagerAdapter, UserManagerAdapter>()
            .AddPersistence()
            .AddGemini(configuration);
}