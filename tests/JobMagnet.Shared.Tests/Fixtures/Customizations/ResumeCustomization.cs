using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ResumeCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() => new Headline(
            FixtureBuilder.Faker.Name.Prefix(),
            "",
            FixtureBuilder.Faker.Name.JobTitle(),
            FixtureBuilder.Faker.Lorem.Paragraph(),
            FixtureBuilder.Faker.Lorem.Paragraph(),
            FixtureBuilder.Faker.Lorem.Paragraph(),
            FixtureBuilder.Faker.Address.FullAddress()
        ));

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
}