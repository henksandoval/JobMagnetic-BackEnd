using AutoFixture;
using Bogus;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class CareerHistoryCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<CareerHistory>(composer =>
            composer
                .FromFactory(() => CareerHistory.CreateInstance(
                    _guidGenerator,
                    new ProfileId(),
                    Faker.Lorem.Paragraph()))
                .OmitAutoProperties()
        );

        fixture.Customize<CareerHistoryBase>(composer =>
            composer
                .With(x => x.Introduction, Faker.Lorem.Paragraph())
                .OmitAutoProperties()
        );

        fixture.Register(() =>
            new SummaryRaw(
                Faker.Lorem.Paragraph(),
                [],
                []
            )
        );
    }
}