using JobMagnet.Domain.Core.Entities.Contact;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ContactTypeAliasEntityTypeConfiguration : IEntityTypeConfiguration<ContactTypeAlias>
{
    public void Configure(EntityTypeBuilder<ContactTypeAlias> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Alias)
            .IsRequired()
            .HasMaxLength(ContactTypeAlias.MaxAliasLength);

        builder.HasIndex(c => new { c.Alias, c.ContactTypeId })
            .IsUnique();
    }
}