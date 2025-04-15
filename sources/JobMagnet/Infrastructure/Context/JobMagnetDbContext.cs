using System.Security.Claims;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Entities.Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Context;

public class JobMagnetDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
{
    public DbSet<ProfileEntity> Profiles { get; set; }
    public DbSet<EducationEntity> Educations { get; set; }
    public DbSet<PortfolioEntity> Portfolios { get; set; }
    public DbSet<TalentEntity> Talents { get; set; }
    public DbSet<PortfolioGalleryItemEntity> PortfolioGalleries { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<ServiceGalleryItemEntity> ServiceGalleries { get; set; }
    public DbSet<ResumeEntity> Resumes { get; set; }
    public DbSet<SkillItemEntity> SkillSets { get; set; }
    public DbSet<SkillEntity> Skills { get; set; }
    public DbSet<SummaryEntity> Summaries { get; set; }
    public DbSet<TestimonialEntity> Testimonials { get; set; }
    public DbSet<WorkExperienceEntity> WorkExperiences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProfileEntity>().ToTable("Profiles");
        modelBuilder.Entity<EducationEntity>().ToTable("Educations");
        modelBuilder.Entity<PortfolioEntity>().ToTable("Porfolios");
        modelBuilder.Entity<TalentEntity>().ToTable("Talents");
        modelBuilder.Entity<PortfolioGalleryItemEntity>().ToTable("PorfolioGalleryItems");
        modelBuilder.Entity<ServiceEntity>().ToTable("Services");
        modelBuilder.Entity<ServiceGalleryItemEntity>().ToTable("ServiceGalleryItems");
        modelBuilder.Entity<ResumeEntity>().ToTable("Resumes");
        modelBuilder.Entity<SkillItemEntity>().ToTable("SkillItems");
        modelBuilder.Entity<SkillEntity>().ToTable("Skills");
        modelBuilder.Entity<SummaryEntity>().ToTable("Summaries");
        modelBuilder.Entity<TestimonialEntity>().ToTable("Testimonials");
        modelBuilder.Entity<WorkExperienceEntity>().ToTable("WorkExperiences");
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

    private Guid GetCurrentUserId()
    {
        var userIdString = httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userIdString, out var userId) ? userId : Guid.Empty;
    }
}