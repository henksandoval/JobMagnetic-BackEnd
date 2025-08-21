using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ProfileHeaderEntityTypeConfiguration : IEntityTypeConfiguration<ProfileHeader>
{
    public void Configure(EntityTypeBuilder<ProfileHeader> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => new ProfileHeaderId(value))
            .ValueGeneratedNever();

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(r => r.JobTitle)
            .HasMaxLength(ProfileHeader.MaxJobTitleLength);

        builder.HasMany(r => r.ContactInfo)
            .WithOne()
            .HasForeignKey(contact => contact.ProfileHeaderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}