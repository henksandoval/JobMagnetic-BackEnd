using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ContactTypeEntityTypeConfiguration : IEntityTypeConfiguration<ContactType>
{
    public void Configure(EntityTypeBuilder<ContactType> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(ContactType.MaxNameLength);

        builder.Property(c => c.IconClass)
            .HasMaxLength(ContactType.MaxIconClassLength);

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasData(ContactTypeSeeder.SeedData.Types);
    }
}