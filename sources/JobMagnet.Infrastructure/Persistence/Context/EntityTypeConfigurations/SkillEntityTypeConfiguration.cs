using JobMagnet.Domain.Core.Entities.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillEntityTypeConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasOne<SkillType>()
            .WithMany()
            .HasForeignKey(s => s.SkillTypeId)
            .IsRequired();

        builder.HasOne<SkillSet>()
            .WithMany()
            .HasForeignKey(s => s.SkillSetId)
            .IsRequired();
    }
}