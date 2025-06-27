using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Skills.Entities;

public readonly record struct SkillTypeId(Guid Value) : IStronglyTypedId<Guid>;

public class SkillType : SoftDeletableAggregate<SkillTypeId>
{
    public const int MaxNameLength = 50;
    private const string DefaultIconUri = "https://jobmagnet.com/default-icon.png";

    private readonly List<SkillTypeAlias> _aliases = [];

    public string Name { get; private set; }
    public Uri IconUrl { get; private set; }
    public SkillCategoryId CategoryId { get; private set; }

    public virtual IReadOnlyCollection<SkillTypeAlias> Aliases => _aliases.AsReadOnly();
    public virtual SkillCategory Category { get; private set; }

    private SkillType(SkillTypeId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt, DateTimeOffset? deletedAt) :
        base(id, addedAt, lastModifiedAt, deletedAt)
    {
    }

    private SkillType(SkillTypeId id, IClock clock, string name, SkillCategory category, Uri? iconUrl = null) : base(id, clock)
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

    internal static SkillType CreateInstance(IGuidGenerator guidGenerator, IClock clock, string name, SkillCategory category, Uri? iconUrl = null)
    {
        var id = new SkillTypeId(guidGenerator.NewGuid());
        return new SkillType(id, clock, name, category, iconUrl);
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