using System.Reflection;
using JobMagnet.Application.Services;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Base.Interfaces;
using JobMagnet.Domain.Core.Entities.Contact;
using JobMagnet.Domain.Core.Entities.Skills;
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
    public DbSet<ContactTypeAlias> ContactAliases { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<VanityUrl> PublicProfileIdentifier { get; set; }
    public DbSet<EducationEntity> Educations { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Talent> Talents { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<ProfessionalSummary> Summaries { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
    public DbSet<WorkExperienceEntity> WorkExperiences { get; set; }
    public DbSet<WorkResponsibilityEntity> WorkResponsibilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Talent>().ToTable("Talents");
        modelBuilder.Entity<ProfessionalSummary>().ToTable("Summaries");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        UpdateEntityProperties();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateEntityProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateEntityProperties()
    {
        var currentUserId = GetCurrentUserId();
        foreach (var entry in ChangeTracker.Entries())
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is IAuditableEntity auditableEntityAdded)
                    {
                        auditableEntityAdded.AddedAt = DateTime.UtcNow;
                        auditableEntityAdded.AddedBy = currentUserId;
                    }

                    break;

                case EntityState.Modified:
                    if (entry.Entity is IAuditableEntity auditableEntityModified)
                    {
                        auditableEntityModified.LastModifiedAt = DateTime.UtcNow;
                        auditableEntityModified.LastModifiedBy = currentUserId;
                    }

                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDeletableEntity<object> softDeletableEntity)
                    {
                        softDeletableEntity.IsDeleted = true;
                        softDeletableEntity.DeletedAt = DateTime.UtcNow;
                        softDeletableEntity.DeletedBy = currentUserId;

                        entry.State = EntityState.Modified;
                    }

                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(entry.State),
                        entry.State,
                        "The entity state is not supported in this context."
                    );
            }
    }

    private Guid GetCurrentUserId() => currentUserService.GetCurrentUserId() ?? Guid.Empty;
}