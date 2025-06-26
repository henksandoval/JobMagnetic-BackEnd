using JobMagnet.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class WorkResponsibilityEntityTypeConfiguration : IEntityTypeConfiguration<WorkResponsibility>
{
    public void Configure(EntityTypeBuilder<WorkResponsibility> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(WorkResponsibility.MaxDescriptionLength);
    }
}