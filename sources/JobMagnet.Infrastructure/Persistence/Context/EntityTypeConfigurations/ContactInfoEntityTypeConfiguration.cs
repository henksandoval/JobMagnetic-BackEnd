using JobMagnet.Domain.Core.Entities.ContactInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class ContactInfoEntityTypeConfiguration : IEntityTypeConfiguration<ContactInfoEntity>
{
    public void Configure(EntityTypeBuilder<ContactInfoEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value)
            .IsRequired();

        builder.HasOne<ContactTypeEntity>(x => x.ContactType)
            .WithMany()
            .HasForeignKey(x => x.ContactTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}