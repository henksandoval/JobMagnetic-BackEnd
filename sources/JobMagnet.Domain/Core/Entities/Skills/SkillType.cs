using System.Diagnostics.CodeAnalysis;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities.Skills;

public class SkillType : SoftDeletableEntity<int>
{
    public const int MaxNameLength = 50;

    public string Name { get; private set; }
    public string IconUrl { get; private set; }
    public ushort CategoryId { get; private set; }

    public virtual IReadOnlyCollection<SkillTypeAlias> Aliases => _aliases.AsReadOnly();
    public virtual SkillCategory Category { get; private set; }

    private readonly List<SkillTypeAlias> _aliases = [];

    private SkillType() { }

    [SetsRequiredMembers]
    public SkillType(int id, string name, SkillCategory category, Uri? iconUrl = null)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(category);

        Id = id;
        Name = name;
        Category = category;
        CategoryId = category.Id;
        IconUrl = iconUrl?.AbsoluteUri ?? string.Empty;
    }

    [SetsRequiredMembers]
    public SkillType(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Id = 0;
        Name = name;
    }

    public void AddAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);

        if (_aliases.Any(a => a.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase)))
        {
            throw new JobMagnetDomainException($"The alias ({alias}) already exists.");
        }

        var newAlias = new SkillTypeAlias(alias, Id);
        _aliases.Add(newAlias);
    }

    public void RemoveAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);

        var aliasToRemove = _aliases.FirstOrDefault(a => a.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase));

        aliasToRemove?.Delete();
    }

    public void UpdateIcons(Uri? newIconUrl)
    {
        IconUrl = newIconUrl?.AbsoluteUri;
    }

    public void SetDefaultIcon()
    {
        IconUrl = "https://jobmagnet.com/default-icon.png";
    }
}