using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Contracts.Commands.Resume;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class ResumeMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapResumeEntityToResumeModelCorrectly()
    {
        // --- Given ---
        var entity = _fixture.Create<ResumeEntity>();

        // --- When ---
        var resumeModel = entity.ToModel();

        // --- Then ---
        resumeModel.Should().NotBeNull();
        resumeModel.Id.Should().Be(entity.Id);
        resumeModel.ResumeData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
    }

    [Fact]
    public void MapResumeEntityToResumeUpdateCommandCorrectly()
    {
        // --- Given ---
        var entity = _fixture.Create<ResumeEntity>();

        // --- When ---
        var updateCommand = entity.ToUpdateRequest();

        // --- Then ---
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