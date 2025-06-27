using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class VanityUrlEntityTypeConfiguration : IEntityTypeConfiguration<VanityUrl>
{
    public void Configure(EntityTypeBuilder<VanityUrl> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<VanityUrlId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(x => x.ProfileSlugUrl)
            .IsUnique();

        builder.Property(p => p.Type)
            .IsRequired();

        builder.Property(x => x.ProfileSlugUrl)
            .HasMaxLength(VanityUrl.MaxNameLength)
            .IsRequired();
    }
}