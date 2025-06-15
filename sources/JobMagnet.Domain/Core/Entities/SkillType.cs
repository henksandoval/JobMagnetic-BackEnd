using System.Diagnostics.CodeAnalysis;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities;

public class SkillType : SoftDeletableEntity<int>
{
    public string Name { get; private set; }
    public string Category { get; private set; }
    public string IconUrl { get; private set; }

    public virtual IReadOnlyCollection<SkillTypeAlias> Aliases => _aliases.AsReadOnly();

    private readonly List<SkillTypeAlias> _aliases = [];

    private SkillType() { }

    [SetsRequiredMembers]
    public SkillType(int id, string name, string category, Uri? iconUrl = null)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(category);

        Id = id;
        Name = name;
        Category = category;
        IconUrl = iconUrl?.AbsoluteUri ?? string.Empty;
    }

    [SetsRequiredMembers]
    public SkillType(string name, string category)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(category);

        Name = name;
        Category = category;
    }

    public void AddAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);

        if (_aliases.Any(a => a.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase)))
        {
            throw new JobMagnetDomainException($"The alias ({alias}) already exists.");
        }

        var newAlias = new SkillTypeAlias(alias, this);
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
        IconUrl = null;
    }
}