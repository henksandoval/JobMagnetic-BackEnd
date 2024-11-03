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
        
        
        // Register all IREPOSITORY
        return services;
    }
}