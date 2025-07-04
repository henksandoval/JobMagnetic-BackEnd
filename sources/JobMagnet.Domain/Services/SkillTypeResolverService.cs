using CSharpFunctionalExtensions;
using JobMagnet.Domain.Aggregates.Skills;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Services;

public interface ISkillTypeResolverService
{
    Task<IDictionary<string, Maybe<SkillType>>> ResolveAsync(IEnumerable<string> namesOrAliases, CancellationToken cancellationToken);
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

    public async Task<IDictionary<string, Maybe<SkillType>>> ResolveAsync(IEnumerable<string> namesOrAliases, CancellationToken cancellationToken)
    {
        var uniqueNames = new HashSet<string>(namesOrAliases, StringComparer.CurrentCultureIgnoreCase);
        if (uniqueNames.Count == 0) return new Dictionary<string, Maybe<SkillType>>();

        var foundSkillTypes = await skillTypeRepository.FindAsync(s =>
                uniqueNames.Contains(s.Name) || s.Aliases.Any(a => uniqueNames.Contains(a.Alias)),
            cancellationToken);

        var namesMap = foundSkillTypes
            .Select(skillType => (skillType.Name, SkillType: skillType));

        var aliasesMap = foundSkillTypes
            .SelectMany(skillType => skillType.Aliases
                    .Select(alias => (Name: alias.Alias, SkillType: skillType))
            );

        var resolutionMap = namesMap.Concat(aliasesMap)
            .Where(pair => uniqueNames.Contains(pair.Name))
            .GroupBy(pair => pair.Name, StringComparer.CurrentCultureIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => group.First().SkillType,
                StringComparer.CurrentCultureIgnoreCase
            );

        return uniqueNames.ToDictionary(
            name => name,
            name => resolutionMap.TryGetValue(name, out var skillType)
                ? Maybe.From(skillType)
                : Maybe<SkillType>.None,
            StringComparer.CurrentCultureIgnoreCase
        );
    }
}