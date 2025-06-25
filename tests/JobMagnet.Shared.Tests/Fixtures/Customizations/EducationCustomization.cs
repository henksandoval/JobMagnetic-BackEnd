using AutoFixture;
using Bogus;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class EducationCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<EducationEntity>(composer => composer
            .FromFactory(EducationFactory)
            .OmitAutoProperties());

        fixture.Register(EducationRawFactory);
        fixture.Register(EducationBaseFactory);
    }

    private static EducationEntity EducationFactory()
    {
        var startDate = Faker.Date.Past(20, DateTime.Now.AddYears(-5));
        var endDate = Faker.Date.Between(startDate, startDate.AddYears(5))
            .OrNull(Faker, 0.25f);

        var education = new EducationEntity(
            Faker.PickRandom(StaticCustomizations.Degrees),
            Faker.PickRandom(StaticCustomizations.Universities),
            Faker.Address.FullAddress(),
            startDate,
            endDate,
            Faker.Lorem.Sentences()
        );

        return education;
    }

    private static EducationRaw EducationRawFactory()
    {
        var startDate = Faker.Date.Past(20, DateTime.Now.AddYears(-5));
        var endDate = Faker.Date.Between(startDate, startDate.AddYears(5))
            .OrNull(Faker, 0.25f);

        var education = new EducationRaw(
            Faker.PickRandom(StaticCustomizations.Degrees),
            Faker.PickRandom(StaticCustomizations.Universities),
            Faker.Address.FullAddress(),
            Faker.Lorem.Sentences(),
            startDate.ToDateOnly().ToString(),
            endDate?.ToDateOnly().ToString() ?? string.Empty
        );

        return education;
    }

    private static EducationBase EducationBaseFactory()
    {
        var startDate = Faker.Date.Past(20, DateTime.Now.AddYears(-5));
        var endDate = Faker.Date.Between(startDate, startDate.AddYears(5))
            .OrNull(Faker, 0.25f);

        var education = new EducationBase(
            Faker.PickRandom(StaticCustomizations.Degrees),
            Faker.PickRandom(StaticCustomizations.Universities),
            Faker.Address.FullAddress(),
            Faker.Lorem.Sentences(),
            startDate,
            endDate
        );

        return education;
    }
}