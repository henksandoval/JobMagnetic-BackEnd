using JobMagnet.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class PublicProfileIdentifierEntityTypeConfiguration : IEntityTypeConfiguration<PublicProfileIdentifierEntity>
{
    public void Configure(EntityTypeBuilder<PublicProfileIdentifierEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ProfileSlugUrl)
            .HasDatabaseName("IX_PublicProfileIdentifier_Identifier")
            .IsUnique();

        builder.Property(p => p.Type)
            .IsRequired();

        builder.Property(x => x.ProfileSlugUrl)
            .HasMaxLength(PublicProfileIdentifierEntity.MaxNameLength)
            .IsRequired();
    }
}