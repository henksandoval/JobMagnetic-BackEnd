using System.Reflection;
using JobMagnet.Application.Services;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Domain.Aggregates.SkillTypes.Entities;
using JobMagnet.Domain.Aggregates.SkillTypes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Context;

public class JobMagnetDbContext(DbContextOptions options, ICurrentUserService currentUserService)
    : DbContext(options)
{
    public DbSet<SkillSet> SkillSets { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<SkillType> SkillTypes { get; set; }
    public DbSet<SkillCategory> SkillCategories { get; set; }
    public DbSet<SkillTypeAlias> SkillTypeAliases { get; set; }
    public DbSet<ContactInfo> ContactInfo { get; set; }
    public DbSet<ContactType> ContactTypes { get; set; }
    public DbSet<ContactTypeAlias> ContactTypeAliases { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<VanityUrl> VanityUrls { get; set; }
    public DbSet<AcademicDegree> AcademicDegree { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Talent> Talents { get; set; }
    public DbSet<ProfileHeader> ProfileHeaders { get; set; }
    public DbSet<CareerHistory> CareerHistory { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
    public DbSet<WorkExperience> WorkExperiences { get; set; }
    public DbSet<WorkHighlight> WorkHighlight { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");
    }
}