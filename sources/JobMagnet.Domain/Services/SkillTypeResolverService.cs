using CSharpFunctionalExtensions;
using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Services;

public interface ISkillTypeResolverService
{
    Task<Maybe<SkillType>> ResolveAsync(string nameOrAlias, CancellationToken cancellationToken);
}

public class SkillTypeResolverService(
    IQueryRepository<SkillType, int> skillTypeRepository)
    : ISkillTypeResolverService
{
    public async Task<Maybe<SkillType>> ResolveAsync(string nameOrAlias, CancellationToken cancellationToken)
    {
        var skillType = await skillTypeRepository
            .FirstOrDefaultAsync(s =>
                    s.Name.Equals(nameOrAlias, StringComparison.CurrentCultureIgnoreCase) ||
                    s.Aliases.Any(types => types.Alias.Equals(nameOrAlias, StringComparison.CurrentCultureIgnoreCase)),
                cancellationToken);

        return Maybe.From(skillType);
    }
}