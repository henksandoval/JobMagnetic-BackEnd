using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class ServiceRawCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ServiceRaw>(composer =>
            composer.FromFactory(() => new ServiceRaw(
                FixtureBuilder.Faker.Lorem.Paragraph(),
                FixtureBuilder.Build().CreateMany<GalleryItemRaw>().ToList()
            ))
        );
    }
}