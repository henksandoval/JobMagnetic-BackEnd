using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillTypeAliasEntityTypeConfiguration : IEntityTypeConfiguration<SkillTypeAlias>
{
    public void Configure(EntityTypeBuilder<SkillTypeAlias> builder)
    {
        builder.Property(alias => alias.Alias)
            .HasMaxLength(SkillTypeAlias.MaxAliasLength)
            .IsRequired();

        builder.HasIndex(alias => alias.Alias)
            .IsUnique();

        // builder.HasData(SkillSeeder.SeedData.Aliases);
    }
}