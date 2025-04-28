using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Mappers;
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
        var testimonialModel = PortfolioMapper.ToModel(entity);

        // Then
        testimonialModel.Should().NotBeNull();
        testimonialModel.Id.Should().Be(entity.Id);
        testimonialModel.PortfolioData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    private static Expression<Func<PortfolioGalleryEntity, object>> GetExcludeEntityProperties()
    {
        return e => new { e.Id, e.IsDeleted, e.Profile, e.AddedAt, e.AddedBy, e.DeletedAt, e.DeletedBy, e.LastModifiedAt, e.LastModifiedBy };
    }
}