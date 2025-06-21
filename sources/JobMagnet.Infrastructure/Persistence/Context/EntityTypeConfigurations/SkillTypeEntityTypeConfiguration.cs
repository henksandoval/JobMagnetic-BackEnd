using JobMagnet.Domain.Core.Entities.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillTypeEntityTypeConfiguration : IEntityTypeConfiguration<SkillType>
{
    public void Configure(EntityTypeBuilder<SkillType> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.IconUrl)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .HasOne<SkillCategory>(type => type.Category)
            .WithMany()
            .HasForeignKey(type => type.CategoryId)
            .IsRequired();

        builder.HasMany(type => type.Aliases)
            .WithOne()
            .HasForeignKey(alias => alias.SkillTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}