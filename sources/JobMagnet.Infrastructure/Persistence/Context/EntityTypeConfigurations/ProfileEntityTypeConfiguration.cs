using JobMagnet.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasMany(p => p.PublicProfileIdentifiers)
            .WithOne()
            .HasForeignKey(identifier => identifier.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}