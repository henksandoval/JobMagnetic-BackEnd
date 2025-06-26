using JobMagnet.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class SummaryEntityTypeConfiguration : IEntityTypeConfiguration<ProfessionalSummary>
{
    public void Configure(EntityTypeBuilder<ProfessionalSummary> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(s => s.Education)
            .WithOne()
            .HasForeignKey(e => e.SummaryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}