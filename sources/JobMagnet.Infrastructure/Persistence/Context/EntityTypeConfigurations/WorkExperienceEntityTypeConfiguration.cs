using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class WorkExperienceEntityTypeConfiguration : IEntityTypeConfiguration<WorkExperience>
{
    public void Configure(EntityTypeBuilder<WorkExperience> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<WorkExperienceId, Guid>());

        builder.Property(x => x.CareerHistoryId)
            .HasConversion(new StronglyTypedIdValueConverter<CareerHistoryId, Guid>());

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