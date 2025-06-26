using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Contracts.Commands.Testimonial;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class TestimonialMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapTestimonialEntityToTestimonialModelCorrectly()
    {
        // --- Given ---
        var entity = _fixture.Create<Testimonial>();

        // --- When ---
        var testimonialModel = entity.ToModel();

        // --- Then ---
        testimonialModel.Should().NotBeNull();
        testimonialModel.Id.Should().Be(entity.Id);
        testimonialModel.TestimonialData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    [Fact]
    public void MapTestimonialCommandToTestimonialEntityCorrectly()
    {
        // --- Given ---
        var createCommand = _fixture.Create<TestimonialCommand>();

        // --- When ---
        var entity = createCommand.ToEntity();

        // --- Then ---
        entity.Should().NotBeNull();
        entity.Should().BeEquivalentTo(createCommand.TestimonialData);
    }

    [Fact]
    public void MapTestimonialEntityToTestimonialUpdateCommandCorrectly()
    {
        // --- Given ---
        var entity = _fixture.Create<Testimonial>();

        // --- When ---
        var updateCommand = entity.ToUpdateCommand();

        // --- Then ---
        updateCommand.Should().NotBeNull();
        updateCommand.TestimonialData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    private static Expression<Func<Testimonial, object>> GetExcludeEntityProperties()
    {
        return e => new
        {
            e.Id, e.IsDeleted, e.AddedAt, e.AddedBy, e.DeletedAt, e.DeletedBy, e.LastModifiedAt,
            e.LastModifiedBy
        };
    }
}