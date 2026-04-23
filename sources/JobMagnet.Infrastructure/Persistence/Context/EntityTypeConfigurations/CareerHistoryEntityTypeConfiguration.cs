using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class CareerHistoryEntityTypeConfiguration : IEntityTypeConfiguration<CareerHistory>
{
    public void Configure(EntityTypeBuilder<CareerHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => new CareerHistoryId(value))
            .ValueGeneratedNever();

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(s => s.AcademicDegree)
            .WithOne()
            .HasForeignKey(e => e.CareerHistoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.WorkExperiences)
            .WithOne()
            .HasForeignKey(e => e.CareerHistoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}