using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Mappers;
using JobMagnet.Models.Commands.Portfolio;
using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class PortfolioMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapPorfolioEntityToPortfolioModelCorrectly()
    {
        // Given
        var entity = _fixture.Create<PortfolioGalleryEntity>();

        // When
        var testimonialModel = entity.ToModel();

        // Then
        testimonialModel.Should().NotBeNull();
        testimonialModel.Id.Should().Be(entity.Id);
        testimonialModel.PortfolioData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    [Fact]
    public void MapPortfolioCreateCommandToPortfolioEntityCorrectly()
    {
        // Given
        var createCommand = _fixture.Create<PortfolioCreateCommand>();

        // When
        var entity = createCommand.ToEntity();

        // Then
        entity.Should().NotBeNull();
        entity.Should().BeEquivalentTo(createCommand.PortfolioData);
    }

    [Fact]
    public void MapPortfolioEntityToPortfolioUpdateCommandCorrectly()
    {
        // Given
        var entity = _fixture.Create<PortfolioGalleryEntity>();

        // When
        var updateCommand = PortfolioMapper.ToUpdateRequest(entity);

        // Then
        updateCommand.Should().NotBeNull();
        updateCommand.Id.Should().Be(entity.Id);
        updateCommand.PortfolioData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    private static Expression<Func<PortfolioGalleryEntity, object>> GetExcludeEntityProperties()
    {
        return e => new { e.Id, e.IsDeleted, e.Profile, e.AddedAt, e.AddedBy, e.DeletedAt, e.DeletedBy, e.LastModifiedAt, e.LastModifiedBy };
    }
}