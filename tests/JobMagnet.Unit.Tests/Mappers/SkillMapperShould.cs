using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;

namespace JobMagnet.Unit.Tests.Mappers;

public class SkillMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapSkillEntityToSkillResponseCorrectly()
    {
        // Given
        var profileEntity = new ProfileEntityBuilder(_fixture)
            .WithSkills()
            .WithSkillDetails()
            .Build();
        var entity = profileEntity.Skill!;

        // When
        var skillModel = entity.ToResponse();

        // Then
        skillModel.Should().NotBeNull();
        skillModel.Id.Should().Be(entity.Id);
        skillModel.SkillData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
        skillModel.SkillData.Skills.Should().BeEquivalentTo(entity.Skills, options =>
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
        var entity = _fixture.Create<SkillSetEntity>();

        // When
        var updateCommand = entity.ToUpdateCommand();

        // Then
        updateCommand.Should().NotBeNull();
        updateCommand.SkillData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
        updateCommand.SkillData.Skills.Should().BeEquivalentTo(entity.Skills, options =>
            options.Excluding(GetExcludeItemEntityProperties()));
    }

    private static Expression<Func<SkillSetEntity, object>> GetExcludeEntityProperties()
    {
        return e => new
        {
            e.Id,
            SkillDetails = e.Skills, e.IsDeleted, e.Profile, e.AddedAt, e.AddedBy, e.DeletedAt, e.DeletedBy,
            e.LastModifiedAt, e.LastModifiedBy
        };
    }

    private static Expression<Func<SkillEntity, object>> GetExcludeItemEntityProperties()
    {
        return e => new { e.Id, Skill = e.SkillSet, e.SkillId, e.AddedAt, e.AddedBy, e.LastModifiedAt, e.LastModifiedBy };
    }
}