using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;

namespace JobMagnet.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddLogging()
            .AddTransient(typeof(IQueryRepository<ResumeEntity, long>), typeof(Repository<ResumeEntity, long>))
            .AddTransient(typeof(ICommandRepository<ResumeEntity>), typeof(Repository<ResumeEntity, long>));
    }
}