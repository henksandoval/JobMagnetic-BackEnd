using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
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

        builder.HasOne<ProfileHeader>(p => p.Header)
            .WithOne()
            .HasForeignKey<ProfileHeader>(r => r.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<CareerHistory>(p => p.CareerHistory)
            .WithOne()
            .HasForeignKey<CareerHistory>(s => s.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<SkillSet>(p => p.SkillSet)
            .WithOne()
            .HasForeignKey<SkillSet>(s => s.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.VanityUrls)
            .WithOne()
            .HasForeignKey(identifier => identifier.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Testimonials)
            .WithOne()
            .HasForeignKey(t => t.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p => p.Portfolio)
            .WithOne()
            .HasForeignKey(p => p.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p => p.TalentShowcase)
            .WithOne()
            .HasForeignKey(t => t.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}