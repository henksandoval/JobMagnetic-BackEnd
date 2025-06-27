using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Skills;
using JobMagnet.Domain.Aggregates.Skills.Entities;
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
            // .AddTransient<ISeeder, Seeder>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddQueryRepositories()
            .AddCommandRepositories();

    private static IServiceCollection AddQueryRepositories(this IServiceCollection services) =>
        services
            .AddTransient<IQueryRepository<Headline, long>, Repository<Headline, long>>()
            .AddTransient<IQueryRepository<VanityUrl, long>, Repository<VanityUrl, long>>()
            .AddTransient<IQueryRepository<ContactType, int>, Repository<ContactType, int>>()
            .AddTransient<IQueryRepository<ContactTypeAlias, int>, Repository<ContactTypeAlias, int>>()
            .AddTransient<IQueryRepository<Talent, long>, Repository<Talent, long>>()
            .AddTransient<IQueryRepository<Testimonial, long>, Repository<Testimonial, long>>()
            .AddTransient<IQueryRepository<SkillCategory, ushort>, Repository<SkillCategory, ushort>>()
            .AddTransient<IQueryRepository<SkillType, int>, Repository<SkillType, int>>()
            .AddTransient<IQueryRepository<SkillTypeAlias, int>, Repository<SkillTypeAlias, int>>()
            .AddTransient<IQueryRepository<Project, long>, Repository<Project, long>>()
            .AddTransient<IQueryRepository<Skill, long>, Repository<Skill, long>>()
            .AddTransient<IQueryRepository<CareerHistory, long>, Repository<CareerHistory, long>>()
            .AddTransient<IProfileQueryRepository, ProfileQueryRepository>();

    private static IServiceCollection AddCommandRepositories(this IServiceCollection services) =>
        services
            .AddTransient<ICommandRepository<Profile>, Repository<Profile, long>>()
            .AddTransient<ICommandRepository<VanityUrl>, Repository<VanityUrl, long>>()
            .AddTransient<ICommandRepository<Talent>, Repository<Talent, long>>()
            .AddTransient<ICommandRepository<Headline>, Repository<Headline, long>>()
            .AddTransient<ICommandRepository<Testimonial>, Repository<Testimonial, long>>()
            .AddTransient<ICommandRepository<Project>, Repository<Project, long>>()
            .AddTransient<ICommandRepository<SkillSet>, Repository<SkillSet, long>>()
            .AddTransient<ICommandRepository<CareerHistory>, Repository<CareerHistory, long>>();
}