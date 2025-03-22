using JobMagnet.Repositories;
using JobMagnet.Repositories.Interface;

namespace JobMagnet.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddLogging()
            .AddTransient(typeof(IQueryRepository<>), typeof(Repository<>))
            .AddTransient(typeof(ICommandRepository<>), typeof(Repository<>));
    }
}