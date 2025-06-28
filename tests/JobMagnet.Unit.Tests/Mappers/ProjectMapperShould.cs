using System.Linq.Expressions;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.Portfolio;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles.Entities;

using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class ProjectMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapProjectEntityToProjectModelCorrectly()
    {
        // --- Given ---
        var entity = _fixture.Create<Project>();

        // --- When ---
        var testimonialModel = entity.ToModel();

        // --- Then ---
        testimonialModel.Should().NotBeNull();
        testimonialModel.Id.Should().Be(entity.Id.Value);
        testimonialModel.ProjectData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    [Fact]
    public void MapProjectCreateCommandToProjectEntityCorrectly()
    {
        // --- Given ---
        var createCommand = _fixture.Create<ProjectCommand>();

        // --- When ---
        var entity = createCommand.ToEntity();

        // --- Then ---
        entity.Should().NotBeNull();
        entity.Should().BeEquivalentTo(createCommand.ProjectData);
    }

    [Fact]
    public void MapProjectEntityToProjectCommandCorrectly()
    {
        // --- Given ---
        var entity = _fixture.Create<Project>();

        // --- When ---
        var updateCommand = entity.ToUpdateRequest();

        // --- Then ---
        updateCommand.Should().NotBeNull();
        updateCommand.ProjectData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    private static Expression<Func<Project, object>> GetExcludeEntityProperties()
    {
        return e => new
        {
            e.Id, e.IsDeleted, e.AddedAt, e.AddedBy, e.DeletedAt, e.DeletedBy, e.LastModifiedAt,
            e.LastModifiedBy
        };
    }
}