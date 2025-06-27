using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<ProfileId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne<Headline>(p => p.Resume)
            .WithOne()
            .HasForeignKey<Headline>(r => r.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<CareerHistory>(p => p.ProfessionalSummary)
            .WithOne()
            .HasForeignKey<CareerHistory>(s => s.ProfileId)
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