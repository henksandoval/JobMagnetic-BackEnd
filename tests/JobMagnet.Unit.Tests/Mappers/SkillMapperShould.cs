using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities.Skills;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;

namespace JobMagnet.Unit.Tests.Mappers;

public class SkillMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapSkillEntityToSkillResponseCorrectly()
    {
        // --- Given ---
        var profileEntity = new ProfileEntityBuilder(_fixture)
            .WithSkillSet()
            .WithSkills()
            .Build();
        var entity = profileEntity.SkillSet!;

        var sourceSkills = entity.Skills.OrderBy(s => s.Rank).ToList();

        // --- When ---
        var skillModel = entity.ToResponse();
        var mappedSkills = skillModel.SkillData.Skills.OrderBy(s => s.Rank).ToList();

        // --- Then ---
        skillModel.Should().NotBeNull();
        skillModel.Id.Should().Be(entity.Id);

        skillModel.SkillData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(e => e.Skills).Excluding(GetExcludeEntityProperties()));

        mappedSkills.Should().HaveSameCount(sourceSkills);

        for (var i = 0; i < sourceSkills.Count; i++)
        {
            var sourceSkill = sourceSkills[i];
            var mappedSkill = mappedSkills[i];

            mappedSkill.Id.Should().Be(sourceSkill.Id);
            mappedSkill.ProficiencyLevel.Should().Be(sourceSkill.ProficiencyLevel);
            mappedSkill.Rank.Should().Be(sourceSkill.Rank);
            mappedSkill.Name.Should().Be(sourceSkill.SkillType.Name);
            mappedSkill.IconUrl.Should().Be(sourceSkill.SkillType.IconUrl.AbsoluteUri);
        }
    }

    [Fact]
    public void MapSkillEntityToSkillUpdateCommandCorrectly()
    {
        // --- Given ---
        var entity = _fixture.Create<SkillSet>();

        // --- When ---
        var updateCommand = entity.ToUpdateCommand();

        // --- Then ---
        updateCommand.Should().NotBeNull();
        updateCommand.SkillData.Should().BeEquivalentTo(entity, options =>
            options.Excluding(GetExcludeEntityProperties()));
        updateCommand.SkillData.Skills.Should().BeEquivalentTo(entity.Skills, options =>
            options.Excluding(GetExcludeItemEntityProperties()));
    }

    private static Expression<Func<SkillSet, object>> GetExcludeEntityProperties()
    {
        return e => new
        {
            e.Id,
            SkillDetails = e.Skills, e.IsDeleted, e.AddedAt, e.AddedBy, e.DeletedAt, e.DeletedBy,
            e.LastModifiedAt, e.LastModifiedBy
        };
    }

    private static Expression<Func<Skill, object>> GetExcludeItemEntityProperties()
    {
        return e => new { e.Id, SkillId = e.SkillSetId, e.SkillType, e.SkillTypeId, e.AddedAt, e.AddedBy, e.LastModifiedAt, e.LastModifiedBy };
    }
}