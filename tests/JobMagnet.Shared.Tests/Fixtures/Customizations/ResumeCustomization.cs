using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;

using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ResumeCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Register(() => new Headline(
            Faker.Name.Prefix(),
            "",
            Faker.Name.JobTitle(),
            Faker.Lorem.Paragraph(),
            Faker.Lorem.Paragraph(),
            Faker.Lorem.Paragraph(),
            Faker.Address.FullAddress(),
            new HeadlineId(),
            new ProfileId()
        ));

        fixture.Register(() =>
        {
            return new ResumeRaw(
                Faker.Name.JobTitle(),
                Faker.Lorem.Paragraph(),
                Faker.Address.FullAddress(),
                Faker.Lorem.Paragraph(),
                Faker.Lorem.Paragraph(),
                TestUtilities.OptionalValue(Faker, f => f.Name.Prefix()),
                TestUtilities.OptionalValue(Faker, f => f.Name.Suffix()),
                []
            );
        });
    }
}