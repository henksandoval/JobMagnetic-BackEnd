using AutoFixture;
using Bogus;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ContactTypeCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private static readonly List<ContactTypeEntity> ContactTypes = [];

    static ContactTypeCustomization()
    {
        var contactTypesCollection = new ContactTypesCollection().GetContactTypesWithAliases();

        var currentId = 1;

        foreach (var contactType in contactTypesCollection)
        {
            var testContactType = new ContactTypeEntity(currentId++, contactType.Name, contactType.IconClass);

            foreach (var alias in contactType.Aliases)
            {
                testContactType.AddAlias(alias.Alias);
            }

            ContactTypes.Add(testContactType);
        }

    }
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ContactTypeEntity>(composer =>
            composer
                .FromFactory(() => Faker.PickRandom(ContactTypes))
                .OmitAutoProperties());
    }
}