using JobMagnet.Entities;
using JobMagnet.Repositories;
using JobMagnet.Repositories.Interface;
using JobMagnet.Service;
using JobMagnet.Service.Interface;

namespace JobMagnet.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register all IServices
        services
            .AddTransient<IAboutService, AboutService>()
            .AddTransient<ISkillService, SkillService>()
            .AddTransient(typeof(IQueryRepository<>), typeof(Repository<>))
            .AddTransient(typeof(ICommandRepository<>), typeof(Repository<>));

        services.AddLogging();
        return services;
    }
}