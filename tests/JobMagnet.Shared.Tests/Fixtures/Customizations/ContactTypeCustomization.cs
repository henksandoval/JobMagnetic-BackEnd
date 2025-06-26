using AutoFixture;
using Bogus;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ContactTypeCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<ContactType>(composer =>
            composer
                .FromFactory(() => Faker.PickRandom(ContactTypeDataFactory.GetDomainContactTypes().ToList()))
                .OmitAutoProperties());
    }
}