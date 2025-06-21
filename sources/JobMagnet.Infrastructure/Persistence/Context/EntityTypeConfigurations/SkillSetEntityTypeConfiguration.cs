using JobMagnet.Domain.Core.Entities.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillSetEntityTypeConfiguration : IEntityTypeConfiguration<SkillSet>
{
    public void Configure(EntityTypeBuilder<SkillSet> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasMany(s => s.Skills)
            .WithOne(skill => skill.SkillSet)
            .HasForeignKey(skill => skill.SkillSetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}