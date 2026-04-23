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

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => new ContactTypeId(value))
            .ValueGeneratedNever();

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(ContactType.MaxNameLength);

        builder.Property(c => c.IconClass)
            .HasMaxLength(ContactType.MaxIconClassLength);

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasData(ContactTypeSeeder.SeedData.Types);

        builder.OwnsMany(c => c.Aliases,
            typeAliasBuilder =>
            {
                typeAliasBuilder.WithOwner().HasForeignKey(nameof(ContactTypeId));
                typeAliasBuilder.HasKey("Id");
                typeAliasBuilder.Property<Guid>("Id").ValueGeneratedOnAdd();
                typeAliasBuilder.Property(r => r.Alias)
                    .IsRequired()
                    .HasMaxLength(ContactTypeAlias.MaxAliasLength);
                typeAliasBuilder.HasData(ContactTypeSeeder.SeedData.Aliases);
            });
    }
}