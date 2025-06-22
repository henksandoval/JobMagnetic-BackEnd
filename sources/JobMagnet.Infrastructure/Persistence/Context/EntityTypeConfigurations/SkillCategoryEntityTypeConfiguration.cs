using JobMagnet.Domain.Core.Entities.Skills;
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

        builder.HasMany(c => c.SkillTypes)
            .WithOne(type => type.Category)
            .HasForeignKey(type => type.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData( new SkillCategory(SkillCategory.DefaultCategoryName, 1));
    }
}