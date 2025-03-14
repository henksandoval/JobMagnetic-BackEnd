using JobMagnet.Entities;
using JobMagnet.Repository;
using JobMagnet.Repository.Interface;
using JobMagnet.Service;
using JobMagnet.Service.Interface;

namespace JobMagnet.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register all IServices
        services.AddTransient<IAboutService, AboutService>();
        services.AddTransient<ISkillService, SkillService>();
        services.AddTransient<ISummaryService, SummaryService>();
        services.AddScoped<IAboutRepository<AboutEntity>, AboutRepository<AboutEntity>>();
        services.AddScoped<ISkillRepository<SkillEntity>, SkillRepository<SkillEntity>>();

        services.AddLogging();
        return services;
    }
}