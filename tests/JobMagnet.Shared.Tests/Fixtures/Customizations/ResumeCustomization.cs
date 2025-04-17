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
                .Without(x => x.Profile)
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .With(x => x.JobTitle, FixtureBuilder.Faker.Name.JobTitle())
                .With(x => x.About, FixtureBuilder.Faker.Lorem.Paragraph())
                .With(x => x.Address, FixtureBuilder.Faker.Address.FullAddress())
                .With(x => x.Summary, FixtureBuilder.Faker.Lorem.Paragraph())
                .With(x => x.Overview, FixtureBuilder.Faker.Lorem.Paragraph())
                .With(x => x.Title, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Prefix()))
                .With(x => x.Suffix, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Suffix()))
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
                .OmitAutoProperties()
        );
    }
}