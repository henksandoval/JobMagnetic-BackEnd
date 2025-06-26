using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Skills.Entities;

public readonly record struct SkillTypeId(Guid Value) : IStronglyTypedId<Guid>;

public class SkillType : SoftDeletableEntity<SkillTypeId>
{
    public const int MaxNameLength = 50;
    private const string DefaultIconUri = "https://jobmagnet.com/default-icon.png";

    private readonly List<SkillTypeAlias> _aliases = [];

    public string Name { get; private set; }
    public Uri IconUrl { get; private set; }
    public SkillCategoryId CategoryId { get; private set; }

    public virtual IReadOnlyCollection<SkillTypeAlias> Aliases => _aliases.AsReadOnly();
    public virtual SkillCategory Category { get; private set; }

    private SkillType() : base(new SkillTypeId(), Guid.Empty)
    {
    }

    internal SkillType(SkillTypeId id, string name, SkillCategory category, Uri? iconUrl = null) : base(id, Guid.Empty)
    {
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

        var newAlias = new SkillTypeAlias(alias);
        _aliases.Add(newAlias);
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