using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Mappers;
using JobMagnet.Models.Commands.Resume;
using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class ResumeMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapResumeEntityToResumeModelCorrectly()
    {
        // Given
        var entity = _fixture.Create<ResumeEntity>();

        // When
        var resumeModel = entity.ToModel();

        // Then
        resumeModel.Should().NotBeNull();
        resumeModel.Id.Should().Be(entity.Id);
        resumeModel.ResumeData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    [Fact]
    public void MapResumeCommandToResumeEntityCorrectly()
    {
        // Given
        var createCommand = _fixture.Create<ResumeCommand>();

        // When
        var entity = createCommand.ToEntity();

        // Then
        entity.Should().NotBeNull();
        entity.Should().BeEquivalentTo(createCommand.ResumeData, options =>
            options.ExcludingMissingMembers());
    }

    [Fact]
    public void MapResumeEntityToResumeUpdateCommandCorrectly()
    {
        // Given
        var entity = _fixture.Create<ResumeEntity>();

        // When
        var updateCommand = entity.ToUpdateRequest();

        // Then
        updateCommand.Should().NotBeNull();
        updateCommand.ResumeData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    private static Expression<Func<ResumeEntity, object>> GetExcludeEntityProperties()
    {
        return e => new
        {
            e.Id,
            e.IsDeleted,
            e.Profile,
            e.AddedAt,
            e.AddedBy,
            e.DeletedAt,
            e.DeletedBy,
            e.LastModifiedAt,
            e.LastModifiedBy,
            e.Address,
            e.ContactInfo
        };
    }
}