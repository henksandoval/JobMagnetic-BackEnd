using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne<Resume>(p => p.Resume)
            .WithOne()
            .HasForeignKey<Resume>(r => r.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ProfessionalSummary>(p => p.ProfessionalSummary)
            .WithOne()
            .HasForeignKey<ProfessionalSummary>(s => s.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<SkillSet>(p => p.SkillSet)
            .WithOne()
            .HasForeignKey<SkillSet>(s => s.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.LinkManager);
        builder.HasMany(p => p.VanityUrls)
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