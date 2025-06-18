using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ContactInfoCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<ContactInfoEntity>(composer =>
            composer
                .FromFactory((ResumeEntity resume) => BuildContactInfoEntity())
                .OmitAutoProperties());

        fixture.Customize<ContactInfoRaw>(composer =>
            composer
                .FromFactory(BuildContactInfoRaw)
                .OmitAutoProperties());
    }

    private static ContactInfoEntity BuildContactInfoEntity()
    {
        var randomContactTypeId = Faker.Random.Short(1, 6);

        var (typeName, value) = GenerateContactDetails(randomContactTypeId);

        return new ContactInfoEntity(0, value, new ContactTypeEntity(typeName));
    }

    private static ContactInfoRaw BuildContactInfoRaw()
    {
        var randomContactTypeId = Faker.Random.Short(1, 6);

        var (typeName, value) = GenerateContactDetails(randomContactTypeId);

        return new ContactInfoRaw(typeName, value);
    }

    private static (string TypeName, string Value) GenerateContactDetails(short contactTypeId)
    {
        return contactTypeId switch
        {
            1 => ("Email", Faker.Person.Email),
            2 => ("Phone", Faker.Phone.PhoneNumber()),
            3 => ("LinkedIn", $"https://linkedin.com/in/{Faker.Internet.UserName()}"),
            4 => ("GitHub", $"https://github.com/{Faker.Internet.UserName()}"),
            5 => ("Website", Faker.Internet.Url()),
            _ => ("Other", Faker.Internet.DomainName())
        };
    }
}