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
            .SetupConfigureQueryRepositories()
            .SetupConfigureCommandRepositories();
    }

    private static IServiceCollection SetupConfigureQueryRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient(typeof(IQueryRepository<ResumeEntity, long>), typeof(Repository<ResumeEntity, long>))
            .AddTransient(typeof(IQueryRepository<TestimonialEntity, long>),
                typeof(Repository<TestimonialEntity, long>))
            .AddTransient(typeof(IQueryRepository<PortfolioGalleryItemEntity, long>),
                typeof(Repository<PortfolioGalleryItemEntity, long>))
            .AddTransient(typeof(IQueryRepository<SkillItemEntity, long>), typeof(Repository<SkillItemEntity, long>))
            .AddTransient<IPortfolioQueryRepository, PortfolioQueryRepository>()
            .AddTransient<ISkillQueryRepository, SkillQueryRepository>();
    }

    private static IServiceCollection SetupConfigureCommandRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<ICommandRepository<ResumeEntity>, Repository<ResumeEntity, long>>()
            .AddTransient(typeof(ICommandRepository<TestimonialEntity>), typeof(Repository<TestimonialEntity, long>))
            .AddTransient(typeof(ICommandRepository<PortfolioEntity>), typeof(Repository<PortfolioEntity, long>))
            .AddTransient<ICommandRepository<SkillEntity>, Repository<SkillEntity, long>>();
    }
}