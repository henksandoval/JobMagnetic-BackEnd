using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Implements;
using JobMagnet.Infrastructure.Repositories.Interfaces;

namespace JobMagnet.DependencyInjection;

internal static class HostExtensions
{
    internal static IServiceCollection AddHostDependencies(this IServiceCollection services)
    {
        return services
            .AddLogging()
            .AddQueryRepositories()
            .AddCommandRepositories();
    }

    private static IServiceCollection AddQueryRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient(typeof(IQueryRepository<ResumeEntity, long>), typeof(Repository<ResumeEntity, long>))
            .AddTransient(typeof(IQueryRepository<TestimonialEntity, long>),
                typeof(Repository<TestimonialEntity, long>))
            .AddTransient(typeof(IQueryRepository<PortfolioGalleryItemEntity, long>),
                typeof(Repository<PortfolioGalleryItemEntity, long>))
            .AddTransient(typeof(IQueryRepository<SkillItemEntity, long>), typeof(Repository<SkillItemEntity, long>))
            .AddTransient(typeof(IQueryRepository<ServiceGalleryItemEntity, long>), typeof(Repository<ServiceGalleryItemEntity, long>))
            .AddTransient<IPortfolioQueryRepository, PortfolioQueryRepository>()
            .AddTransient<ISkillQueryRepository, SkillQueryRepository>()
            .AddTransient<IServiceQueryRepository, ServiceQueryRepository>();
    }

    private static IServiceCollection AddCommandRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<ICommandRepository<ResumeEntity>, Repository<ResumeEntity, long>>()
            .AddTransient<ICommandRepository<TestimonialEntity>, Repository<TestimonialEntity, long>>()
            .AddTransient<ICommandRepository<PortfolioEntity>, Repository<PortfolioEntity, long>>()
            .AddTransient<ICommandRepository<SkillEntity>, Repository<SkillEntity, long>>()
            .AddTransient<ICommandRepository<ServiceEntity>, Repository<ServiceEntity, long>>();
    }
}