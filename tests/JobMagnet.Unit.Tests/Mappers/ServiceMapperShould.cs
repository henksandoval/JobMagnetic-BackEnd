using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Mappers;
using JobMagnet.Models.Commands.Service;
using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class ServiceMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapServiceEntityToServiceModelCorrectly()
    {
        // Given
        var entity = _fixture.Create<ServiceEntity>();

        // When
        var serviceModel = entity.ToModel();

        // Then
        serviceModel.Should().NotBeNull();
        serviceModel.Id.Should().Be(entity.Id);
        serviceModel.ServiceData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    [Fact]
    public void MapServiceCommandToServiceEntityCorrectly()
    {
        // Given
        var createCommand = _fixture.Create<ServiceCommand>();

        // When
        var entity = createCommand.ToEntity();

        // Then
        entity.Should().NotBeNull();
        entity.Should().BeEquivalentTo(createCommand.ServiceData);
    }

    [Fact]
    public void MapServiceEntityToServiceUpdateCommandCorrectly()
    {
        // Given
        var entity = _fixture.Create<ServiceEntity>();

        // When
        var updateCommand = entity.ToUpdateCommand();

        // Then
        updateCommand.Should().NotBeNull();
        updateCommand.ServiceData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
        updateCommand.ServiceData.GalleryItems.Should().BeEquivalentTo(entity.GalleryItems, options =>
            options.Excluding(GetExcludeGalleryEntityProperties()));
    }

    private static Expression<Func<ServiceEntity, object>> GetExcludeEntityProperties()
    {
        return e => new { e.Id, e.IsDeleted, e.Profile, e.GalleryItems, e.AddedAt, e.AddedBy, e.DeletedAt, e.DeletedBy, e.LastModifiedAt, e.LastModifiedBy };
    }

    private static Expression<Func<ServiceGalleryItemEntity, object>> GetExcludeGalleryEntityProperties()
    {
        return e => new { e.Id, e.Service, e.ServiceId, e.AddedAt, e.AddedBy, e.LastModifiedAt, e.LastModifiedBy };
    }
}