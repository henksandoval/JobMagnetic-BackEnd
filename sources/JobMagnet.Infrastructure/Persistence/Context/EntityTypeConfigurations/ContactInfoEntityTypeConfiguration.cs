using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ContactInfoEntityTypeConfiguration : IEntityTypeConfiguration<ContactInfo>
{
    public void Configure(EntityTypeBuilder<ContactInfo> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => new ContactInfoId(value))
            .ValueGeneratedNever();

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(x => x.Value)
            .IsRequired();

        builder.HasOne<ContactType>(info => info.ContactType)
            .WithMany()
            .HasForeignKey(info => info.ContactTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}