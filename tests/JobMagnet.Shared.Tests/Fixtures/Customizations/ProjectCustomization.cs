using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ProjectCustomization : ICustomization
{
    private static int _autoIncrementId = 1;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<Project>(composer =>
            composer
                .FromFactory(() => new Project(
                    FixtureBuilder.Faker.Company.CompanyName(),
                    FixtureBuilder.Faker.Lorem.Sentence(),
                    FixtureBuilder.Faker.Image.PicsumUrl(),
                    FixtureBuilder.Faker.Image.PicsumUrl(),
                    FixtureBuilder.Faker.Image.PicsumUrl(),
                    FixtureBuilder.Faker.Address.CountryCode()
                ))
                .OmitAutoProperties()
        );

        fixture.Customize<ProjectRaw>(composer =>
            composer.FromFactory(() => new ProjectRaw(
                FixtureBuilder.Faker.Company.CompanyName(),
                FixtureBuilder.Faker.Lorem.Sentence(),
                FixtureBuilder.Faker.Image.PicsumUrl(),
                FixtureBuilder.Faker.Image.PicsumUrl(),
                FixtureBuilder.Faker.Image.PicsumUrl(),
                FixtureBuilder.Faker.Address.CountryCode()
            ))
        );
    }
}