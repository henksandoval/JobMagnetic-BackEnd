using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Domain.Aggregates.Auth.Entities;
using JobMagnet.Domain.Aggregates.Auth.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id)
            .HasConversion(id => id.Value, value => new RefreshTokenId(value))
            .ValueGeneratedNever();
        
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<RefreshToken>(u => u.UserId);
        
        builder.HasIndex(rt => rt.Token).IsUnique();
    }
}