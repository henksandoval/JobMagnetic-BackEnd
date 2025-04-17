using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ResumeCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ResumeEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .Without(x => x.ContactInfo)
                .Without(x => x.Profile)
                // .With(x => x.ContactInfo, fixture.CreateMany<ContactInfoEntity>().ToList())
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
                .OmitAutoProperties()
        );
    }
}