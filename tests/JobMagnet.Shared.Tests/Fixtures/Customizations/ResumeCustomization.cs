using AutoFixture;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Shared.Tests.Utils;

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
                .With(x => x.ProfileId, 0)
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.JobTitle = FixtureBuilder.Faker.Name.JobTitle();
        item.About = FixtureBuilder.Faker.Lorem.Paragraph();
        item.Address = FixtureBuilder.Faker.Address.FullAddress();
        item.Summary = FixtureBuilder.Faker.Lorem.Paragraph();
        item.Overview = FixtureBuilder.Faker.Lorem.Paragraph();
        item.Title = TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Prefix());
        item.Suffix = TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Suffix());
    }
}