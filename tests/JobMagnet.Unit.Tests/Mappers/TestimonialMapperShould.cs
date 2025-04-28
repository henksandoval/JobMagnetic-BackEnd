using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Mappers;
using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class TestimonialMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapTestimonialEntityToTestimonialModelCorrectly()
    {
        // Given
        var entity = _fixture.Create<TestimonialEntity>();

        // When
        var result = entity.ToModel();

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.TestimonialData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    private static Expression<Func<TestimonialEntity, object>> GetExcludeEntityProperties()
    {
        return e => new { e.Id, e.IsDeleted, e.Profile, e.AddedAt, e.AddedBy, e.DeletedAt, e.DeletedBy, e.LastModifiedAt, e.LastModifiedBy };
    }
}