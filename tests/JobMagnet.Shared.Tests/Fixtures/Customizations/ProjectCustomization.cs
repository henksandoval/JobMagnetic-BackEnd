using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ProjectCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private static int _autoIncrementId = 1;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<Project>(composer =>
            composer
                .FromFactory(() => new Project(
                    Faker.Company.CompanyName(),
                    Faker.Lorem.Sentence(),
                    Faker.Image.PicsumUrl(),
                    Faker.Image.PicsumUrl(),
                    Faker.Image.PicsumUrl(),
                    Faker.Address.CountryCode(),
                    _autoIncrementId++,
                    new ProfileId(),
                    new ProjectId()
                ))
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