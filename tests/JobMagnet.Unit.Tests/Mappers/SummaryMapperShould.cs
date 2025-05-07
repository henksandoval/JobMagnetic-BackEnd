using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Mappers;
using JobMagnet.Models.Commands.Summary;
using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class SummaryMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapSummaryEntityToSummaryModelCorrectly()
    {
        // Given
        var entity = _fixture.Create<SummaryEntity>();

        // When
        var summaryModel = entity.ToModel();

        // Then
        summaryModel.Should().NotBeNull();
        summaryModel.Id.Should().Be(entity.Id);
        summaryModel.SummaryData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
        summaryModel.SummaryData.Education.Should().BeEquivalentTo(entity.Education, options =>
            options.Excluding(GetExcludeEducationEntityProperties()));
        summaryModel.SummaryData.WorkExperiences.Should().BeEquivalentTo(entity.WorkExperiences, options =>
            options.Excluding(GetExcludeWorkExperienceEntityProperties()));
    }

    [Fact]
    public void MapSummaryCreateCommandToSummaryEntityCorrectly()
    {
        // Given
        var createCommand = _fixture.Create<SummaryCreateCommand>();

        // When
        var entity = createCommand.ToEntity();

        // Then
        entity.Should().NotBeNull();
        entity.Should().BeEquivalentTo(createCommand.SummaryData);
    }
/*
    [Fact]
    public void MapSummaryEntityToSummaryUpdateCommandCorrectly()
    {
        // Given
        var entity = _fixture.Create<SummaryEntity>();

        // When
        var updateCommand = entity.ToUpdateCommand();

        // Then
        updateCommand.Should().NotBeNull();
        updateCommand.Id.Should().Be(entity.Id);
        updateCommand.SummaryData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
        updateCommand.SummaryData.SummaryDetails.Should().BeEquivalentTo(entity.SummaryDetails, options =>
            options.Excluding(GetExcludeItemEntityProperties()));
    }


*/
    private static Expression<Func<SummaryEntity, object>> GetExcludeEntityProperties()
    {
        return e => new
        {
            e.Id, e.Education, e.WorkExperiences, e.IsDeleted, e.Profile, e.AddedAt, e.AddedBy, e.DeletedAt, e.DeletedBy, e.LastModifiedAt, e.LastModifiedBy
        };
    }

    private static Expression<Func<EducationEntity, object>> GetExcludeEducationEntityProperties()
    {
        return e => new { e.Id, e.Summary, e.SummaryId, e.AddedAt, e.AddedBy, e.LastModifiedAt, e.LastModifiedBy, e.IsDeleted, e.DeletedAt, e.DeletedBy };
    }

    private static Expression<Func<WorkExperienceEntity, object>> GetExcludeWorkExperienceEntityProperties()
    {
        return e => new { e.Id, e.Summary, e.SummaryId, e.AddedAt, e.AddedBy, e.LastModifiedAt, e.LastModifiedBy, e.IsDeleted, e.DeletedAt, e.DeletedBy };
    }
}