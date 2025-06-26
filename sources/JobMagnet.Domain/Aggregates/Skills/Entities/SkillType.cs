using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities.Skills;

public class SkillType : SoftDeletableEntity<int>
{
    public const int MaxNameLength = 50;
    private const string DefaultIconUri = "https://jobmagnet.com/default-icon.png";

    private readonly List<SkillTypeAlias> _aliases = [];

    public string Name { get; private set; }
    public Uri IconUrl { get; private set; }
    public ushort CategoryId { get; private set; }

    public virtual IReadOnlyCollection<SkillTypeAlias> Aliases => _aliases.AsReadOnly();
    public virtual SkillCategory Category { get; private set; }

    private SkillType()
    {
    }

    [SetsRequiredMembers]
    internal SkillType(string name, SkillCategory category, int id = 0, Uri? iconUrl = null)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsNotNullOrWhiteSpace(name);
        Guard.HasSizeLessThanOrEqualTo(name, MaxNameLength);
        Guard.IsNotNull(category);

        iconUrl ??= new Uri(DefaultIconUri);

        Id = id;
        Name = name;
        Category = category;
        IconUrl = iconUrl;
    }

    public void AddAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);

        if (_aliases.Any(a => a.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase)))
            throw new JobMagnetDomainException($"The alias ({alias}) already exists.");

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
        IconUrl = newIconUrl ?? throw new ArgumentNullException(nameof(newIconUrl), "Icon URL cannot be null.");
    }

    public void SetCategory(SkillCategory category)
    {
        Guard.IsNotNull(category);

        Category = category;
    }
}