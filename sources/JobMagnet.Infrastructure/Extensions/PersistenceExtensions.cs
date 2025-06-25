using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Contact;
using JobMagnet.Domain.Core.Entities.Skills;
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
            .AddTransient<IQueryRepository<ContactType, int>, Repository<ContactType, int>>()
            .AddTransient<IQueryRepository<ContactTypeAlias, int>, Repository<ContactTypeAlias, int>>()
            .AddTransient<IQueryRepository<TalentEntity, long>, Repository<TalentEntity, long>>()
            .AddTransient<IQueryRepository<TestimonialEntity, long>, Repository<TestimonialEntity, long>>()
            .AddTransient<IQueryRepository<SkillCategory, ushort>, Repository<SkillCategory, ushort>>()
            .AddTransient<IQueryRepository<SkillType, int>, Repository<SkillType, int>>()
            .AddTransient<IQueryRepository<SkillTypeAlias, int>, Repository<SkillTypeAlias, int>>()
            .AddTransient<IQueryRepository<Project, long>, Repository<Project, long>>()
            .AddTransient<IQueryRepository<Skill, long>, Repository<Skill, long>>()
            .AddTransient<IQueryRepository<SummaryEntity, long>, Repository<SummaryEntity, long>>()
            .AddTransient<IProfileQueryRepository, ProfileQueryRepository>()
            .AddTransient<ISkillQueryRepository, SkillQueryRepository>()
            .AddTransient<ISummaryQueryRepository, SummaryQueryRepository>();
    }

    private static IServiceCollection AddCommandRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<ICommandRepository<ProfileEntity>, Repository<ProfileEntity, long>>()
            .AddTransient<ICommandRepository<PublicProfileIdentifierEntity>, Repository<PublicProfileIdentifierEntity, long>>()
            .AddTransient<ICommandRepository<TalentEntity>, Repository<TalentEntity, long>>()
            .AddTransient<ICommandRepository<ResumeEntity>, Repository<ResumeEntity, long>>()
            .AddTransient<ICommandRepository<TestimonialEntity>, Repository<TestimonialEntity, long>>()
            .AddTransient<ICommandRepository<Project>, Repository<Project, long>>()
            .AddTransient<ICommandRepository<SkillSet>, Repository<SkillSet, long>>()
            .AddTransient<ICommandRepository<SummaryEntity>, Repository<SummaryEntity, long>>();
    }
}