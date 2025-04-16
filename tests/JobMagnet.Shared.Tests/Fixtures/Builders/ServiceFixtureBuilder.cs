using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public static class ServiceFixtureBuilder
{
    public static ServiceEntity BuildServiceEntity(this IFixture fixture, int serviceItems = 5)
    {
        var serviceGalleryItems = fixture.CreateMany<ServiceGalleryItemEntity>(serviceItems).ToList();
        var serviceEntity = fixture.Build<ServiceEntity>()
            .With(x => x.Id, 0)
            .With(x => x.Overview, FixtureBuilder.Faker.Lorem.Paragraph())
            .With(x => x.IsDeleted, false)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .With(x => x.Profile, fixture.GetProfileEntityComposer().Create())
            .With(x => x.GalleryItems, serviceGalleryItems)
            .Create();

        return serviceEntity;
    }
}