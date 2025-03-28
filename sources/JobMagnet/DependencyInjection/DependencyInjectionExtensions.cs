using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Implements;
using JobMagnet.Infrastructure.Repositories.Interfaces;

namespace JobMagnet.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddLogging()
            .AddTransient(typeof(IQueryRepository<ResumeEntity, long>), typeof(Repository<ResumeEntity, long>))
            .AddTransient(typeof(ICommandRepository<ResumeEntity>), typeof(Repository<ResumeEntity, long>))
            .AddTransient(typeof(IQueryRepository<TestimonialEntity, long>), typeof(Repository<TestimonialEntity, long>))
            .AddTransient(typeof(ICommandRepository<TestimonialEntity>), typeof(Repository<TestimonialEntity, long>))
            .AddTransient<IPortfolioQueryRepository, PortfolioQueryRepository>()
            .AddTransient(typeof(ICommandRepository<PortfolioEntity>), typeof(Repository<PortfolioEntity, long>));
    }
}