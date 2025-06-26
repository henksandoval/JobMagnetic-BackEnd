using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SummaryEntityTypeConfiguration : IEntityTypeConfiguration<CareerHistory>
{
    public void Configure(EntityTypeBuilder<CareerHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(s => s.Qualifications)
            .WithOne()
            .HasForeignKey(e => e.SummaryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}