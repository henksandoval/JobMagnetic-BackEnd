using JobMagnet.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class WorkResponsibilityEntityTypeConfiguration : IEntityTypeConfiguration<WorkResponsibilityEntity>
{
    public void Configure(EntityTypeBuilder<WorkResponsibilityEntity> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(WorkResponsibilityEntity.MaxDescriptionLength);
    }
}