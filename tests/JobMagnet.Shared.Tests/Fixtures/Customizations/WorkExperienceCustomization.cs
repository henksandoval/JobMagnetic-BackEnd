using AutoFixture;
using Bogus;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class WorkExperienceCustomization : ICustomization
{
    private readonly IClock _clock = new DeterministicClock();
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<WorkExperience>(composer =>
            composer
                .FromFactory(WorkExperienceFactory)
                .OmitAutoProperties());

        fixture.Register(WorkExperienceRawFactory);
        fixture.Register(WorkExperienceBaseFactory);
    }

    private WorkExperience WorkExperienceFactory()
    {
        var startDate = Faker.Date.Past(20, DateTime.Now.AddYears(-5));
        var endDate = Faker.Date.Between(startDate, startDate.AddYears(5))
            .OrNull(Faker, 0.25f);

        var workExperience = WorkExperience.CreateInstance(
            _guidGenerator,
            _clock,
            new HeadlineId(),
            Faker.PickRandom(StaticCustomizations.JobTitles),
            Faker.PickRandom(StaticCustomizations.CompanyNames),
            Faker.Address.FullAddress(),
            startDate,
            endDate,
            Faker.Lorem.Sentences()
        );

        return workExperience;
    }

    private static WorkExperienceRaw WorkExperienceRawFactory()
    {
        var startDate = Faker.Date.Past(20, DateTime.Now.AddYears(-5));
        var endDate = Faker.Date.Between(startDate, startDate.AddYears(5))
            .OrNull(Faker, 0.25f);

        var workExperience = new WorkExperienceRaw(
            Faker.PickRandom(StaticCustomizations.JobTitles),
            Faker.PickRandom(StaticCustomizations.CompanyNames),
            Faker.Address.FullAddress(),
            startDate.ToDateOnly().ToString(),
            endDate?.ToDateOnly().ToString() ?? string.Empty,
            Faker.Lorem.Sentences(),
            []
        );

        return workExperience;
    }

    private static WorkExperienceBase WorkExperienceBaseFactory()
    {
        var startDate = Faker.Date.Past(20, DateTime.Now.AddYears(-5));
        var endDate = Faker.Date.Between(startDate, startDate.AddYears(5))
            .OrNull(Faker, 0.25f);

        var workExperience = new WorkExperienceBase(
            Faker.PickRandom(StaticCustomizations.JobTitles),
            Faker.PickRandom(StaticCustomizations.CompanyNames),
            Faker.Address.FullAddress(),
            startDate,
            endDate,
            Faker.Lorem.Sentences(),
            []
        );

        return workExperience;
    }
}