using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ContactTypeEntityTypeConfiguration : IEntityTypeConfiguration<ContactType>
{
    public void Configure(EntityTypeBuilder<ContactType> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<ContactTypeId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(ContactType.MaxNameLength);

        builder.Property(c => c.IconClass)
            .HasMaxLength(ContactType.MaxIconClassLength);

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.OwnsMany(c => c.Aliases,
            typeAliasBuilder =>
            {
                typeAliasBuilder.WithOwner().HasForeignKey(nameof(ContactTypeId));
                typeAliasBuilder.HasKey("Id");
                typeAliasBuilder.Property(r => r.Alias)
                    .IsRequired()
                    .HasMaxLength(ContactTypeAlias.MaxAliasLength);
            });

        // builder.HasData(ContactTypeSeeder.SeedData.Types);
        //builder.HasData(SkillSeeder.SeedData.Aliases); //TODO: PENDING TO DEFINE
        /*
         * // En ContactTypeEntityTypeConfiguration.cs

// 1. Necesitas una forma de generar IDs únicos para el seeding.
var aliasSeedId = -1;

// 2. Prepara los datos de semilla para los alias.
var aliasSeedData = ContactTypeSeeder.SeedData.Aliases
    .Select(seed => new
    {
        // El objeto anónimo debe tener las propiedades que EF necesita:
        // la FK y las propiedades del Value Object.
        ContactTypeId = new ContactTypeId(seed.ContactTypeId),
        Alias = seed.Alias,
        Id = aliasSeedId-- // ¡Asignamos el ID negativo explícito!
    }).ToList();

// 3. Dentro del builder.OwnsMany, después de la configuración:
ownedBuilder.HasData(aliasSeedData);
         */
    }
}