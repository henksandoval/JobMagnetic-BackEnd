using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class CareerHistoryEntityTypeConfiguration : IEntityTypeConfiguration<CareerHistory>
{
    public void Configure(EntityTypeBuilder<CareerHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<CareerHistoryId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(s => s.Qualifications)
            .WithOne()
            .HasForeignKey(e => e.CareerHistoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.WorkExperiences)
            .WithOne()
            .HasForeignKey(e => e.CareerHistoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}