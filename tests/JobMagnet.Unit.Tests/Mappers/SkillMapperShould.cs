using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Commands.Skill;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class SkillMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapSkillEntityToSkillModelCorrectly()
    {
        // Given
        var entity = _fixture.Create<SkillEntity>();

        // When
        var skillModel = entity.ToModel();

        // Then
        skillModel.Should().NotBeNull();
        skillModel.Id.Should().Be(entity.Id);
        skillModel.SkillData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
        skillModel.SkillData.SkillDetails.Should().BeEquivalentTo(entity.SkillDetails, options =>
            options.Excluding(GetExcludeItemEntityProperties()));
    }

    [Fact]
    public void MapSkillCommandToSkillEntityCorrectly()
    {
        // Given
        var createCommand = _fixture.Create<SkillCommand>();

        // When
        var entity = createCommand.ToEntity();

        // Then
        entity.Should().NotBeNull();
        entity.Should().BeEquivalentTo(createCommand.SkillData);
    }

    [Fact]
    public void MapSkillEntityToSkillUpdateCommandCorrectly()
    {
        // Given
        var entity = _fixture.Create<SkillEntity>();

        // When
        var updateCommand = entity.ToUpdateCommand();

        // Then
        updateCommand.Should().NotBeNull();
        updateCommand.SkillData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
        updateCommand.SkillData.SkillDetails.Should().BeEquivalentTo(entity.SkillDetails, options =>
            options.Excluding(GetExcludeItemEntityProperties()));
    }

    private static Expression<Func<SkillEntity, object>> GetExcludeEntityProperties()
    {
        return e => new
        {
            e.Id, e.SkillDetails, e.IsDeleted, e.Profile, e.AddedAt, e.AddedBy, e.DeletedAt, e.DeletedBy, e.LastModifiedAt, e.LastModifiedBy
        };
    }

    private static Expression<Func<SkillItemEntity, object>> GetExcludeItemEntityProperties()
    {
        return e => new { e.Id, e.Skill, e.SkillId, e.AddedAt, e.AddedBy, e.LastModifiedAt, e.LastModifiedBy };
    }
}