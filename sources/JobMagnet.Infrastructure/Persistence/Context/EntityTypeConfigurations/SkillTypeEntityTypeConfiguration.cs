using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Domain.Aggregates.SkillTypes.Entities;
using JobMagnet.Domain.Aggregates.SkillTypes.ValueObjects;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillTypeEntityTypeConfiguration : IEntityTypeConfiguration<SkillType>
{
    public void Configure(EntityTypeBuilder<SkillType> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => new SkillTypeId(value))
            .ValueGeneratedNever();

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

        builder.HasData(SkillSeeder.SeedData.Types);

        builder.OwnsMany(c => c.Aliases,
            typeAliasBuilder =>
            {
                typeAliasBuilder.WithOwner().HasForeignKey(nameof(SkillTypeId));
                typeAliasBuilder.HasKey("Id");
                typeAliasBuilder.Property<Guid>("Id").ValueGeneratedOnAdd();
                typeAliasBuilder.Property(r => r.Alias)
                    .IsRequired()
                    .HasMaxLength(SkillTypeAlias.MaxAliasLength);
                typeAliasBuilder.HasData(SkillSeeder.SeedData.Aliases);
            });
    }
}