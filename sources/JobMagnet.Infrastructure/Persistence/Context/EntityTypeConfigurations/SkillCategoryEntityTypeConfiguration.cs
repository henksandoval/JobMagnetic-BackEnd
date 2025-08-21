using JobMagnet.Domain.Aggregates.SkillTypes.Entities;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillCategoryEntityTypeConfiguration : IEntityTypeConfiguration<SkillCategory>
{
    public void Configure(EntityTypeBuilder<SkillCategory> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => new SkillCategoryId(value))
            .ValueGeneratedNever();

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(c => c.Name)
            .HasMaxLength(SkillCategory.MaxNameLength)
            .IsRequired();

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasData(SkillSeeder.SeedData.Categories);
    }
}