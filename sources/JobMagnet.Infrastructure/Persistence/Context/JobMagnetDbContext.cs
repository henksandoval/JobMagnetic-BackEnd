using JobMagnet.Application.Services;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Context;

public class JobMagnetDbContext(DbContextOptions options, ICurrentUserService currentUserService)
    : DbContext(options)
{
    public DbSet<ContactTypeEntity> ContactTypes { get; set; }
    public DbSet<ContactTypeAliasEntity> ContactAliases { get; set; }
    public DbSet<SkillType> SkillTypes { get; set; }
    public DbSet<SkillCategory> SkillCategories { get; set; }
    public DbSet<SkillTypeAlias> SkillTypeAliases { get; set; }
    public DbSet<ProfileEntity> Profiles { get; set; }
    public DbSet<PublicProfileIdentifierEntity> PublicProfileIdentifier { get; set; }
    public DbSet<EducationEntity> Educations { get; set; }
    public DbSet<PortfolioGalleryEntity> PortfolioGalleries { get; set; }
    public DbSet<TalentEntity> Talents { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<ServiceGalleryItemEntity> ServiceGalleries { get; set; }
    public DbSet<ResumeEntity> Resumes { get; set; }
    public DbSet<ContactInfoEntity> ContactInfo { get; set; }
    public DbSet<SkillEntity> SkillSets { get; set; }
    public DbSet<SkillSetEntity> Skills { get; set; }
    public DbSet<SummaryEntity> Summaries { get; set; }
    public DbSet<TestimonialEntity> Testimonials { get; set; }
    public DbSet<WorkExperienceEntity> WorkExperiences { get; set; }
    public DbSet<WorkResponsibilityEntity> WorkResponsibilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProfileEntity>(entity =>
        {
            entity
                .ToTable("Profiles")
                .HasKey(profile => profile.Id);
        });
        modelBuilder.Entity<ContactTypeEntity>().ToTable("ContactTypes");
        modelBuilder.Entity<EducationEntity>().ToTable("Educations");
        modelBuilder.Entity<PortfolioGalleryEntity>().ToTable("PorfolioGalleryItems");
        modelBuilder.Entity<TalentEntity>().ToTable("Talents");
        modelBuilder.Entity<ServiceEntity>().ToTable("Services");
        modelBuilder.Entity<ServiceGalleryItemEntity>().ToTable("ServiceGalleryItems");
        modelBuilder.Entity<ResumeEntity>().ToTable("Resumes");
        modelBuilder.Entity<ContactInfoEntity>().ToTable("ContactInfo");
        modelBuilder.Entity<SkillEntity>().ToTable("SkillItems");
        modelBuilder.Entity<SkillSetEntity>().ToTable("Skills");
        modelBuilder.Entity<SummaryEntity>().ToTable("Summaries");
        modelBuilder.Entity<TestimonialEntity>().ToTable("Testimonials");
        modelBuilder.Entity<WorkExperienceEntity>().ToTable("WorkExperiences");

        modelBuilder.Entity<PublicProfileIdentifierEntity>(entity =>
        {
            entity.ToTable("PublicProfileIdentifiers")
                .HasKey(publicProfile => publicProfile.Id);

            entity
                .Property(publicProfile => publicProfile.ProfileSlugUrl)
                .IsRequired()
                .HasMaxLength(PublicProfileIdentifierEntity.MaxNameLength);

            entity
                .Property(publicProfile => publicProfile.Type)
                .IsRequired();

            entity
                .Property(publicProfile => publicProfile.ProfileId)
                .IsRequired();

            entity
                .HasIndex(publicProfile => publicProfile.ProfileSlugUrl)
                .HasDatabaseName("IX_PublicProfileIdentifier_Identifier")
                .IsUnique();

            entity.HasIndex(publicProfile => publicProfile.ProfileId)
                .HasDatabaseName("IX_PublicProfileIdentifier_ProfileId")
                .IsUnique(false);

            entity
                .HasOne(publicProfile => publicProfile.ProfileEntity)
                .WithMany(profile => profile.PublicProfileIdentifiers)
                .HasForeignKey(publicProfile => publicProfile.ProfileId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SkillType>(entity =>
        {
            entity.ToTable("SkillTypes")
                .HasKey(skillType => skillType.Id);
        });

        modelBuilder.Entity<SkillCategory>(entity =>
        {
            entity.ToTable("SkillCategories")
                .HasKey(category => category.Id);
        });

        modelBuilder.Entity<WorkResponsibilityEntity>(entity =>
        {
            entity.ToTable("WorkResponsibilities")
                .HasKey(workResponsibility => workResponsibility.Id);

            entity
                .Property(workResponsibility => workResponsibility.Description)
                .IsRequired()
                .HasMaxLength(WorkResponsibilityEntity.MaxDescriptionLength);

            entity
                .HasOne(workResponsibility => workResponsibility.WorkExperience)
                .WithMany(workExperience => workExperience.Responsibilities)
                .HasForeignKey(workResponsibility => workResponsibility.WorkExperienceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
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
        return currentUserService.GetCurrentUserId() ?? Guid.Empty;
    }
}