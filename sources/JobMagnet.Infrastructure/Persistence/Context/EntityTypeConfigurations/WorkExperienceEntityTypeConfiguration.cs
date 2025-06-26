using JobMagnet.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class WorkExperienceEntityTypeConfiguration : IEntityTypeConfiguration<WorkExperience>
{
    public void Configure(EntityTypeBuilder<WorkExperience> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(w => w.Responsibilities)
            .WithOne()
            .HasForeignKey(w => w.WorkExperienceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}