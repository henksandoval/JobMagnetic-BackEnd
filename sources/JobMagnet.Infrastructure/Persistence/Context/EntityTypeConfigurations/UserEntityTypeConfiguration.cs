using JobMagnet.Domain.Aggregates;
using JobMagnet.Infrastructure.Persistence.Context.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobMagnet.Infrastructure.Persistence.Context.EntityTypeConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdValueConverter<UserId, Guid>());

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}