using JobMagnet.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ResumeEntityEntityTypeConfiguration : IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(r => r.JobTitle)
            .HasMaxLength(Resume.MaxJobTitleLength);

        builder.HasMany(r => r.ContactInfo)
            .WithOne()
            .HasForeignKey(contact => contact.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}