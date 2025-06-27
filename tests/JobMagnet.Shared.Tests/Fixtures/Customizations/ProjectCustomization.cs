using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ProjectCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private static int _autoIncrementId = 1;
    private readonly IClock _clock = new DeterministicClock();
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<Project>(composer =>
            composer
                .FromFactory(() => Project.CreateInstance(
                    _guidGenerator,
                    _clock,
                    new ProfileId(),
                    Faker.Company.CompanyName(),
                    Faker.Lorem.Sentence(),
                    Faker.Image.PicsumUrl(),
                    Faker.Image.PicsumUrl(),
                    Faker.Image.PicsumUrl(),
                    Faker.Address.CountryCode(),
                    _autoIncrementId++))
                .OmitAutoProperties()
        );

        fixture.Customize<ProjectRaw>(composer =>
            composer.FromFactory(() => new ProjectRaw(
                Faker.Company.CompanyName(),
                Faker.Lorem.Sentence(),
                Faker.Image.PicsumUrl(),
                Faker.Image.PicsumUrl(),
                Faker.Image.PicsumUrl(),
                Faker.Address.CountryCode()
            ))
        );
    }
}