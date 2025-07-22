using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Domain.Aggregates.SkillTypes.Entities;
using JobMagnet.Domain.Aggregates.SkillTypes.ValueObjects;
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
    internal static IServiceCollection AddPersistence(this IServiceCollection services) =>
        services
            .AddTransient<IJobMagnetDbContextFactory, JobMagnetJobMagnetDbContextFactory>()
            .AddTransient<ISeeder, Seeder>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddQueryRepositories()
            .AddCommandRepositories();

    private static IServiceCollection AddQueryRepositories(this IServiceCollection services) =>
        services
            .AddTransient<IQueryRepository<ProfileHeader, long>, Repository<ProfileHeader, long>>()
            .AddTransient<IQueryRepository<VanityUrl, long>, Repository<VanityUrl, long>>()
            .AddTransient<IQueryRepository<ContactType, int>, Repository<ContactType, int>>()
            .AddTransient<IQueryRepository<ContactTypeAlias, int>, Repository<ContactTypeAlias, int>>()
            .AddTransient<IQueryRepository<Testimonial, long>, Repository<Testimonial, long>>()
            .AddTransient<IQueryRepository<SkillCategory, SkillCategoryId>, Repository<SkillCategory, SkillCategoryId>>()
            .AddTransient<IQueryRepository<SkillSet, SkillSetId>, Repository<SkillSet, SkillSetId>>()
            .AddTransient<IQueryRepository<SkillType, int>, Repository<SkillType, int>>()
            .AddTransient<IQueryRepository<SkillTypeAlias, int>, Repository<SkillTypeAlias, int>>()
            .AddTransient<IQueryRepository<Talent, TalentId>, Repository<Talent, TalentId>>()
            .AddTransient<IQueryRepository<Project, ProjectId>, Repository<Project, ProjectId>>()
            .AddTransient<IQueryRepository<Skill, long>, Repository<Skill, long>>()
            .AddTransient<IQueryRepository<CareerHistory, long>, Repository<CareerHistory, long>>()
            .AddTransient<IProfileQueryRepository, ProfileQueryRepository>();

    private static IServiceCollection AddCommandRepositories(this IServiceCollection services) =>
        services
            .AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>))
            .AddTransient<IGenericCommandRepository<Profile>, Repository<Profile, long>>()
            .AddTransient<IGenericCommandRepository<VanityUrl>, Repository<VanityUrl, long>>()
            .AddTransient<IGenericCommandRepository<ProfileHeader>, Repository<ProfileHeader, long>>()
            .AddTransient<IGenericCommandRepository<Testimonial>, Repository<Testimonial, long>>()
            .AddTransient<IGenericCommandRepository<Project>, Repository<Project, long>>()
            .AddTransient<IGenericCommandRepository<SkillSet>, Repository<SkillSet, long>>()
            .AddTransient<IGenericCommandRepository<CareerHistory>, Repository<CareerHistory, long>>();
}