using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class WorkExperienceEntityTypeConfiguration : IEntityTypeConfiguration<WorkExperience>
{
    public void Configure(EntityTypeBuilder<WorkExperience> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsMany(w => w.Highlights,
            navigationBuilder =>
            {
                navigationBuilder.WithOwner().HasForeignKey("WorkExperienceId");
                navigationBuilder.HasKey("Id");
                navigationBuilder.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(WorkHighlight.MaxDescriptionLength);
            });
    }
}