using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class WorkExperienceEntityTypeConfiguration : IEntityTypeConfiguration<WorkExperience>
{
    public void Configure(EntityTypeBuilder<WorkExperience> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => new WorkExperienceId(value))
            .ValueGeneratedNever();

        builder.Property(u => u.CareerHistoryId)
            .HasConversion(id => id.Value, value => new CareerHistoryId(value))
            .ValueGeneratedNever();

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(w => w.Highlights,
            responsibilityBuilder =>
            {
                responsibilityBuilder.WithOwner().HasForeignKey(nameof(WorkExperienceId));
                responsibilityBuilder.HasKey("Id");
                responsibilityBuilder.Property<Guid>("Id").ValueGeneratedOnAdd();
                responsibilityBuilder.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(WorkHighlight.MaxDescriptionLength);
            });
    }
}