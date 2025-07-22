using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles.Entities;

using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class SummaryMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapSummaryEntityToSummaryModelCorrectly()
    {
        // --- Given ---
        var entity = _fixture.Create<CareerHistory>();

        // --- When ---
        var summaryModel = entity.ToModel();

        // --- Then ---
        summaryModel.Should().NotBeNull();
        summaryModel.Id.Should().Be(entity.Id.Value);
        summaryModel.SummaryData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
        summaryModel.SummaryData.Education.Should().BeEquivalentTo(entity.Qualifications, options =>
            options.Excluding(GetExcludeEducationEntityProperties()));
        summaryModel.SummaryData.WorkExperiences.Should().BeEquivalentTo(entity.WorkExperiences, options =>
            options.Excluding(GetExcludeWorkExperienceEntityProperties()));
    }

    [Fact]
    public void MapSummaryEntityToSummaryUpdateCommandCorrectly()
    {
        // --- Given ---
        var entity = _fixture.Create<CareerHistory>();

        // --- When ---
        var updateCommand = entity.ToUpdateCommand();

        // --- Then ---
        updateCommand.Should().NotBeNull();
        updateCommand.SummaryData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
        updateCommand.SummaryData.Education.Should().BeEquivalentTo(entity.Qualifications, options =>
            options.Excluding(GetExcludeEducationEntityProperties()));
        updateCommand.SummaryData.WorkExperiences.Should().BeEquivalentTo(entity.WorkExperiences, options =>
            options.Excluding(GetExcludeWorkExperienceEntityProperties()));
    }

    private static Expression<Func<CareerHistory, object>> GetExcludeEntityProperties()
    {
        return e => new
        {
            e.Id,
            Education = e.Qualifications, e.WorkExperiences, e.IsDeleted, e.AddedAt, e.AddedBy, e.DeletedAt,
            e.DeletedBy, e.LastModifiedAt, e.LastModifiedBy
        };
    }

    private static Expression<Func<Qualification, object>> GetExcludeEducationEntityProperties()
    {
        return e => new
        {
            e.Id,
            SummaryId = e.HeadlineId, e.AddedAt, e.AddedBy, e.LastModifiedAt, e.LastModifiedBy, e.IsDeleted,
            e.DeletedAt, e.DeletedBy
        };
    }

    private static Expression<Func<WorkExperience, object>> GetExcludeWorkExperienceEntityProperties()
    {
        return e => new
        {
            e.Id,
            SummaryId = e.HeadlineId, e.AddedAt, e.AddedBy, e.LastModifiedAt, e.LastModifiedBy, e.IsDeleted,
            e.DeletedAt, e.DeletedBy
        };
    }
}