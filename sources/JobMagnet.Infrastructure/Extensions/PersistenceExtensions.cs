using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Repositories;
using JobMagnet.Infrastructure.Persistence.Repositories.Base;
using JobMagnet.Infrastructure.Persistence.Seeders;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Infrastructure.Extensions;

internal static class PersistenceExtensions
{
    internal static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        return services
            .AddTransient<IJobMagnetDbContextFactory, JobMagnetJobMagnetDbContextFactory>()
            .AddTransient<ISeeder, Seeder>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddQueryRepositories()
            .AddCommandRepositories();
    }

    private static IServiceCollection AddQueryRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<IQueryRepository<ResumeEntity, long>, Repository<ResumeEntity, long>>()
            .AddTransient<IQueryRepository<PublicProfileIdentifierEntity, long>, Repository<PublicProfileIdentifierEntity, long>>()
            .AddTransient<IQueryRepository<ContactTypeEntity, int>, Repository<ContactTypeEntity, int>>()
            .AddTransient<IQueryRepository<ContactTypeAliasEntity, int>, Repository<ContactTypeAliasEntity, int>>()
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
            .AddTransient<ICommandRepository<PublicProfileIdentifierEntity>, Repository<PublicProfileIdentifierEntity, long>>()
            .AddTransient<ICommandRepository<ResumeEntity>, Repository<ResumeEntity, long>>()
            .AddTransient<ICommandRepository<TestimonialEntity>, Repository<TestimonialEntity, long>>()
            .AddTransient<ICommandRepository<PortfolioGalleryEntity>, Repository<PortfolioGalleryEntity, long>>()
            .AddTransient<ICommandRepository<SkillEntity>, Repository<SkillEntity, long>>()
            .AddTransient<ICommandRepository<ServiceEntity>, Repository<ServiceEntity, long>>()
            .AddTransient<ICommandRepository<SummaryEntity>, Repository<SummaryEntity, long>>();
    }
}