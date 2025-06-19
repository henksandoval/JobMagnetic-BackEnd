using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;
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
                .With(x => x.ProfileId, 0)
                .Without(x => x.Profile)
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );

        fixture.Register(() =>
        {
            return new ResumeRaw(
                FixtureBuilder.Faker.Name.JobTitle(),
                FixtureBuilder.Faker.Lorem.Paragraph(),
                FixtureBuilder.Faker.Address.FullAddress(),
                FixtureBuilder.Faker.Lorem.Paragraph(),
                FixtureBuilder.Faker.Lorem.Paragraph(),
                TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Prefix()),
                TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Suffix()),
                []
            );
        });
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