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
        builder.HasKey(s => s.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => new UserId(value))
            .ValueGeneratedNever();

        var navigation = builder.Metadata.FindNavigation(nameof(User.RefreshTokens));
        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PhotoUrl);
    }
}