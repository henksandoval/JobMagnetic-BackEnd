using JobMagnet.Domain.Core.Entities.Skills;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillCategoryEntityTypeConfiguration : IEntityTypeConfiguration<SkillCategory>
{
    public void Configure(EntityTypeBuilder<SkillCategory> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(SkillCategory.MaxNameLength)
            .IsRequired();

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasData(SkillDataFactory.SeedData.Categories);
    }
}