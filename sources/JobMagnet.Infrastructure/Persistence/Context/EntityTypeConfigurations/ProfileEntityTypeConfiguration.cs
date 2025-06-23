using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne<ResumeEntity>(p => p.Resume)
            .WithOne()
            .HasForeignKey<ResumeEntity>(r => r.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.PublicProfileIdentifiers)
            .WithOne()
            .HasForeignKey(identifier => identifier.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.SkillSet)
            .WithOne(s => s.Profile)
            .HasForeignKey<SkillSet>(s => s.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}