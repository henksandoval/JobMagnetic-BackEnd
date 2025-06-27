using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class HeadlineEntityTypeConfiguration : IEntityTypeConfiguration<Headline>
{
    public void Configure(EntityTypeBuilder<Headline> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<HeadlineId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(r => r.JobTitle)
            .HasMaxLength(Headline.MaxJobTitleLength);

        builder.HasMany(r => r.ContactInfo)
            .WithOne()
            .HasForeignKey(contact => contact.HeadlineId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}