using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class TalentEntityTypeConfiguration : IEntityTypeConfiguration<Talent>
{
    public void Configure(EntityTypeBuilder<Talent> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<TalentId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}