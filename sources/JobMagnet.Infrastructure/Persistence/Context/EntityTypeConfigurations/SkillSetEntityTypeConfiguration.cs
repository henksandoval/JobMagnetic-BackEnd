using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillSetEntityTypeConfiguration : IEntityTypeConfiguration<SkillSet>
{
    public void Configure(EntityTypeBuilder<SkillSet> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<SkillSetId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(s => s.Skills)
            .WithOne()
            .HasForeignKey(skill => skill.SkillSetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}