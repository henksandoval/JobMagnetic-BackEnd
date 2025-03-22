using JobMagnet.Repositories;
using JobMagnet.Repositories.Interface;
using JobMagnet.Service;
using JobMagnet.Service.Interface;

namespace JobMagnet.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddLogging()
            .AddTransient<ISkillService, SkillService>()
            .AddTransient(typeof(IQueryRepository<>), typeof(Repository<>))
            .AddTransient(typeof(ICommandRepository<>), typeof(Repository<>));

        return services;
    }
}