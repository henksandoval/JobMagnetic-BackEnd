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

        builder.OwnsOne(p => p.Name, name =>
        {
            name.Property(n => n.FirstName)
                .HasColumnName(nameof(PersonName.FirstName))
                .HasMaxLength(PersonName.MaxNameLength);

            name.Property(n => n.MiddleName)
                .HasColumnName(nameof(PersonName.MiddleName))
                .HasMaxLength(PersonName.MaxNameLength);

            name.Property(n => n.LastName)
                .HasColumnName(nameof(PersonName.LastName))
                .HasMaxLength(PersonName.MaxNameLength);

            name.Property(n => n.SecondLastName)
                .HasColumnName(nameof(PersonName.SecondLastName))
                .HasMaxLength(PersonName.MaxNameLength);
        });

        builder.OwnsOne(p => p.BirthDate, birthDate =>
        {
            birthDate.Property(b => b.Value)
                .HasColumnName(nameof(BirthDate));
        });

        builder.OwnsOne(p => p.ProfileImage, image =>
        {
            image.Property(i => i.Url)
                .HasColumnName(nameof(ProfileImage))
                .HasConversion(
                    url => url != null ? url.ToString() : null,
                    value => value != null ? new Uri(value) : null
                );
        });

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