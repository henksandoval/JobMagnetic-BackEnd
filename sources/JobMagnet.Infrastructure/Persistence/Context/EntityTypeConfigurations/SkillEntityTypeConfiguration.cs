using JobMagnet.Domain.Aggregates.Skills;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SkillEntityTypeConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<SkillId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(skill => skill.SkillType)
            .WithMany()
            .HasForeignKey(s => s.SkillTypeId)
            .IsRequired();
    }
}