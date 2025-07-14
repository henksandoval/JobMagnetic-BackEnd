using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class QualificationEntityTypeConfiguration : IEntityTypeConfiguration<AcademicDegree>
{
    public void Configure(EntityTypeBuilder<AcademicDegree> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<AcademicDegreeId, Guid>());

        builder.Property(x => x.CareerHistoryId)
            .HasConversion(new StronglyTypedIdValueConverter<CareerHistoryId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}