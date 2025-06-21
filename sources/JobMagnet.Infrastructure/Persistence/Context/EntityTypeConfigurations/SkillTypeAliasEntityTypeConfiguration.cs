using JobMagnet.Domain.Core.Entities.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillTypeAliasEntityTypeConfiguration : IEntityTypeConfiguration<SkillTypeAlias>
{
    public void Configure(EntityTypeBuilder<SkillTypeAlias> builder)
    {
        builder.HasKey(alias => alias.Id);

        builder.Property(alias => alias.Alias)
            .HasMaxLength(50)
            .IsRequired();
    }
}