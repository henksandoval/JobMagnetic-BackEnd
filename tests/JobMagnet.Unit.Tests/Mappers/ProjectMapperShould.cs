using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class ProjectMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapProjectToProjectModelCorrectly()
    {
        // --- Given ---
        var entity = _fixture.Create<Project>();

        // --- When ---
        var model = entity.ToModel();

        // --- Then ---
        model.Should().NotBeNull();
        model.Id.Should().Be(entity.Id.Value);
        model.ProjectData.Should().BeEquivalentTo(entity, options =>
            options.ExcludingMissingMembers()
        );
    }
}