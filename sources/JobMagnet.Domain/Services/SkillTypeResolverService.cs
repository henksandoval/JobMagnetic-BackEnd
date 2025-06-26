using CSharpFunctionalExtensions;
using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Services;

public interface ISkillTypeResolverService
{
    Task<Maybe<SkillType>> ResolveAsync(string nameOrAlias, CancellationToken cancellationToken);
}

public class SkillTypeResolverService(
    IQueryRepository<SkillType, int> skillTypeRepository,
    IQueryRepository<SkillTypeAlias, int> skillTypeAliasRepository)
    : ISkillTypeResolverService
{
    public async Task<Maybe<SkillType>> ResolveAsync(string nameOrAlias, CancellationToken cancellationToken)
    {
        var skillType = await skillTypeRepository
            .FirstOrDefaultAsync(c =>
                    c.Name.ToLower() == nameOrAlias.ToLower(),
                cancellationToken);

        if (skillType is not null) return Maybe.From(skillType);

        var alias = await skillTypeAliasRepository.FirstOrDefaultAsync(
            a => a.Alias.ToLower() == nameOrAlias.ToLower(),
            cancellationToken);

        if (alias is null || !alias.SkillTypeExist) return Maybe.None;

        var finalSkillType = await skillTypeRepository.GetByIdAsync(alias.SkillTypeId, cancellationToken);

        return Maybe.From(finalSkillType);
    }
}