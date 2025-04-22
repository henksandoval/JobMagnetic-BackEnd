using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Implements;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Infrastructure.Seeders;

namespace JobMagnet.Extensions;

internal static class HostExtensions
{
    internal static IServiceCollection AddHostDependencies(this IServiceCollection services)
    {
        return services
            .AddLogging()
            .AddDependencies()
            .AddQueryRepositories()
            .AddCommandRepositories();
    }

    private static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        return services
            .AddTransient<ISeeder, Seeder>();
    }

    private static IServiceCollection AddQueryRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<IQueryRepository<ResumeEntity, long>, Repository<ResumeEntity, long>>()
            .AddTransient<IQueryRepository<TestimonialEntity, long>, Repository<TestimonialEntity, long>>()
            .AddTransient<IQueryRepository<PortfolioGalleryEntity, long>,
                Repository<PortfolioGalleryEntity, long>>()
            .AddTransient<IQueryRepository<SkillItemEntity, long>, Repository<SkillItemEntity, long>>()
            .AddTransient<IQueryRepository<ServiceGalleryItemEntity, long>,
                Repository<ServiceGalleryItemEntity, long>>()
            .AddTransient<IQueryRepository<SummaryEntity, long>, Repository<SummaryEntity, long>>()
            .AddTransient<IProfileQueryRepository, ProfileQueryRepository>()
            .AddTransient<ISkillQueryRepository, SkillQueryRepository>()
            .AddTransient<IServiceQueryRepository, ServiceQueryRepository>()
            .AddTransient<ISummaryQueryRepository, SummaryQueryRepository>();
    }

    private static IServiceCollection AddCommandRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<ICommandRepository<ProfileEntity>, Repository<ProfileEntity, long>>()
            .AddTransient<ICommandRepository<ResumeEntity>, Repository<ResumeEntity, long>>()
            .AddTransient<ICommandRepository<TestimonialEntity>, Repository<TestimonialEntity, long>>()
            .AddTransient<ICommandRepository<PortfolioGalleryEntity>, Repository<PortfolioGalleryEntity, long>>()
            .AddTransient<ICommandRepository<SkillEntity>, Repository<SkillEntity, long>>()
            .AddTransient<ICommandRepository<ServiceEntity>, Repository<ServiceEntity, long>>()
            .AddTransient<ICommandRepository<SummaryEntity>, Repository<SummaryEntity, long>>();
    }
}