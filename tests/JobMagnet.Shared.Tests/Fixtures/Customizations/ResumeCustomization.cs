using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ResumeCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private readonly IClock _clock = new DeterministicClock();
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();

    public void Customize(IFixture fixture)
    {
        fixture.Register(() => ProfileHeader.CreateInstance(
            _guidGenerator,
            _clock,
            new ProfileId(),
            Faker.Name.Prefix(),
            "",
            Faker.Name.JobTitle(),
            Faker.Lorem.Paragraph(),
            Faker.Lorem.Paragraph(),
            Faker.Lorem.Paragraph(),
            Faker.Address.FullAddress()));

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