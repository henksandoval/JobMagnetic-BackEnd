using JobMagnet.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ResumeEntityEntityTypeConfiguration : IEntityTypeConfiguration<ResumeEntity>
{
    public void Configure(EntityTypeBuilder<ResumeEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(r => r.Title)
            .HasMaxLength(ResumeEntity.MaxTitleLength);

        builder.HasMany(r => r.ContactInfo)
            .WithOne()
            .HasForeignKey(contact => contact.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}