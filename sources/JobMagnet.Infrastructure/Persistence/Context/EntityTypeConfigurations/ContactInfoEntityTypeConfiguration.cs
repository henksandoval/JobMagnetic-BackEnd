using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ContactInfoEntityTypeConfiguration : IEntityTypeConfiguration<ContactInfo>
{
    public void Configure(EntityTypeBuilder<ContactInfo> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<ContactInfoId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(x => x.Value)
            .IsRequired();

        builder.HasOne<ContactType>(info => info.ContactType)
            .WithMany()
            .HasForeignKey(info => info.ContactTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}