using AutoFixture;
using Bogus;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ContactTypeCustomization : ICustomization
{
    private static readonly List<(string Name, string IconClass)> ContactTypes = new()
    {
        ("Email", "bx bx-envelope"),
        ("Mobile Phone", "bx bx-mobile"),
        ("Home Phone", "bx bx-phone"),
        ("Work Phone", "bx bx-phone-call"),
        ("Website", "bx bx-globe"),
        ("LinkedIn", "bx bxl-linkedin"),
        ("GitHub", "bx bxl-github"),
        ("Twitter", "bx bxl-twitter"),
        ("Facebook", "bx bxl-facebook"),
        ("Instagram", "bx bxl-instagram"),
        ("YouTube", "bx bxl-youtube"),
        ("WhatsApp", "bx bxl-whatsapp"),
        ("Telegram", "bx bxl-telegram"),
        ("Snapchat", "bx bxl-snapchat"),
        ("Pinterest", "bx bxl-pinterest"),
        ("Skype", "bx bxl-skype"),
        ("Discord", "bx bxl-discord"),
        ("Twitch", "bx bxl-twitch"),
        ("TikTok", "bx bxl-tiktok"),
        ("Reddit", "bx bxl-reddit"),
        ("Vimeo", "bx bxl-vimeo")
    };

    private readonly Faker _faker = new();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<ContactTypeEntity>(composer =>
            composer
                .Without(x => x.Id)
                .With(x => x.Name, _faker.PickRandom(ContactTypes).Name)
                .With(x => x.IconClass, _faker.PickRandom(ContactTypes).IconClass)
                .OmitAutoProperties());
    }
}