using JobMagnet.Domain.Aggregates;
using JobMagnet.Infrastructure.ExternalServices.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => new UserId(value))
            .ValueGeneratedNever();

        builder.Property(u => u.ApplicationIdentityUserId)
            .IsRequired();

        builder.HasOne<ApplicationIdentityUser>()
            .WithOne(i => i.User)
            .HasForeignKey<User>(u => u.ApplicationIdentityUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PhotoUrl);

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}