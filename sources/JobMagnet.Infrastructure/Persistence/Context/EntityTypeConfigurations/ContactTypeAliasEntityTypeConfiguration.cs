using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ContactTypeAliasEntityTypeConfiguration : IEntityTypeConfiguration<ContactTypeAlias>
{
    public void Configure(EntityTypeBuilder<ContactTypeAlias> builder)
    {
        builder.Property(c => c.Alias)
            .IsRequired()
            .HasMaxLength(ContactTypeAlias.MaxAliasLength);

        builder.HasIndex(c => c.Alias)
            .IsUnique();

        builder.HasData(ContactTypeSeeder.SeedData.Aliases);
    }
}