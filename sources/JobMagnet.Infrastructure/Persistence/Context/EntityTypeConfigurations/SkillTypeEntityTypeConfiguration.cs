using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillTypeEntityTypeConfiguration : IEntityTypeConfiguration<SkillType>
{
    public void Configure(EntityTypeBuilder<SkillType> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<SkillTypeId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(s => s.Name)
            .HasMaxLength(SkillType.MaxNameLength)
            .IsRequired();

        builder.Property(s => s.IconUrl)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .HasOne<SkillCategory>(type => type.Category)
            .WithMany()
            .HasForeignKey(type => type.CategoryId)
            .IsRequired();

        builder.OwnsMany(c => c.Aliases,
            typeAliasBuilder =>
            {
                typeAliasBuilder.WithOwner().HasForeignKey(nameof(SkillTypeId));
                typeAliasBuilder.HasKey("Id");
                typeAliasBuilder.Property(r => r.Alias)
                    .IsRequired()
                    .HasMaxLength(SkillTypeAlias.MaxAliasLength);
            });

        // builder.HasData(SkillSeeder.SeedData.Types);
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