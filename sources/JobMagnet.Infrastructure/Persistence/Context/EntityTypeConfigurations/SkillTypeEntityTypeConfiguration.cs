using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillTypeEntityTypeConfiguration : IEntityTypeConfiguration<SkillType>
{
    public void Configure(EntityTypeBuilder<SkillType> builder)
    {
        builder.HasKey(s => s.Id);

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

        builder.HasData(SkillDataFactory.SeedData.Types);
    }
}