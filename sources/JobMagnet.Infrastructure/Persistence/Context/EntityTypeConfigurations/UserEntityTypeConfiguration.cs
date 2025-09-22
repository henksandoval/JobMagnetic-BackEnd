using JobMagnet.Domain.Aggregates;
using JobMagnet.Domain.Aggregates.Auth.Entities;
using JobMagnet.Domain.Aggregates.Auth.ValueObjects;
using JobMagnet.Infrastructure.ExternalServices.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(s => s.Id);
        
        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => new UserId(value))
            .ValueGeneratedNever();
        
        builder.HasOne<ApplicationIdentityUser>()
            .WithOne(i => i.User)
            .HasForeignKey<User>(u => u.ApplicationIdentityUserId);
        
        builder.HasMany(u => u.RefreshTokens)
            .WithOne()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PhotoUrl);
    }
}