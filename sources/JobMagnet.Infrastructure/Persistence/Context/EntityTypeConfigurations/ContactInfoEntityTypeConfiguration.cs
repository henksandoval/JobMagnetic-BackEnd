using JobMagnet.Domain.Core.Entities.Contact;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ContactInfoEntityTypeConfiguration : IEntityTypeConfiguration<ContactInfo>
{
    public void Configure(EntityTypeBuilder<ContactInfo> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value)
            .IsRequired();

        builder.HasOne<ContactType>(info => info.ContactType)
            .WithMany()
            .HasForeignKey(info => info.ContactTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}