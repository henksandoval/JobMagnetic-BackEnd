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
    internal static IServiceCollection AddPersistence(this IServiceCollection services) =>
        services
            .AddTransient<IJobMagnetDbContextFactory, JobMagnetJobMagnetDbContextFactory>()
            .AddTransient<ISeeder, Seeder>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddQueryRepositories()
            .AddCommandRepositories();

    private static IServiceCollection AddQueryRepositories(this IServiceCollection services) =>
        services
            .AddTransient<IQueryRepository<Resume, long>, Repository<Resume, long>>()
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
            .AddTransient<IQueryRepository<ProfessionalSummary, long>, Repository<ProfessionalSummary, long>>()
            .AddTransient<IProfileQueryRepository, ProfileQueryRepository>()
            .AddTransient<ISkillQueryRepository, SkillQueryRepository>()
            .AddTransient<ISummaryQueryRepository, SummaryQueryRepository>();

    private static IServiceCollection AddCommandRepositories(this IServiceCollection services) =>
        services
            .AddTransient<ICommandRepository<Profile>, Repository<Profile, long>>()
            .AddTransient<ICommandRepository<VanityUrl>, Repository<VanityUrl, long>>()
            .AddTransient<ICommandRepository<Talent>, Repository<Talent, long>>()
            .AddTransient<ICommandRepository<Resume>, Repository<Resume, long>>()
            .AddTransient<ICommandRepository<Testimonial>, Repository<Testimonial, long>>()
            .AddTransient<ICommandRepository<Project>, Repository<Project, long>>()
            .AddTransient<ICommandRepository<SkillSet>, Repository<SkillSet, long>>()
            .AddTransient<ICommandRepository<ProfessionalSummary>, Repository<ProfessionalSummary, long>>();
}