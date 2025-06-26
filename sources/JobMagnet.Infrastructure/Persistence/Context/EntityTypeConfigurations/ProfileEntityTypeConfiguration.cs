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

        builder.HasOne<SummaryEntity>(p => p.Summary)
            .WithOne()
            .HasForeignKey<SummaryEntity>(s => s.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<SkillSet>(p => p.SkillSet)
            .WithOne()
            .HasForeignKey<SkillSet>(s => s.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.VanityUrls);
        builder.HasMany(p => p.PublicProfileIdentifiers)
            .WithOne()
            .HasForeignKey(identifier => identifier.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.SocialProof);
        builder.HasMany(p => p.Testimonials)
            .WithOne()
            .HasForeignKey(t => t.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.TalentShowcase);
        builder.HasMany(p => p.Talents)
            .WithOne()
            .HasForeignKey(t => t.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.Portfolio);
        builder.HasMany(p => p.Projects)
            .WithOne()
            .HasForeignKey(p => p.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}