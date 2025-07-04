using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Aggregates;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Contact;

public readonly record struct ContactTypeId(Guid Value) : IStronglyTypedId<ContactTypeId>;

public class ContactType : SoftDeletableAggregate<ContactTypeId>
{
    public const int MaxNameLength = 20;
    public const int MaxIconClassLength = 20;
    public const string DefaultIconClass = "bx bx-link-alt";

    private readonly HashSet<ContactTypeAlias> _aliases = new(new ContactTypeAliasComparer());

    public string Name { get; private set; }
    public string? IconClass { get; private set; }
    public Uri? IconUrl { get; private set; }
    public virtual IReadOnlyCollection<ContactTypeAlias> Aliases => _aliases;

    private ContactType() : base() { }

    private ContactType(ContactTypeId id, IClock clock, string name, string? iconClass = null, Uri? iconUrl = null)
        : base(id, clock)
    {
        Guard.IsNotNullOrWhiteSpace(name);

        Name = name;
        IconClass = iconClass;
        IconUrl = iconUrl;

        EnsureDefaultIcon();
        ValidateInvariants();
    }

    public static ContactType CreateInstance(IGuidGenerator guidGenerator, IClock clock, string name, string? iconClass = null, Uri? iconUrl = null)
    {
        var id = new ContactTypeId(guidGenerator.NewGuid());
        return new ContactType(id, clock, name, iconClass, iconUrl);
    }

    public void UpdateName(string newName, IClock clock)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        Name = newName;
        UpdateModificationDetails(clock);
    }

    public void AddAlias(string aliasValue, IClock clock)
    {
        var newAlias = new ContactTypeAlias(aliasValue);

        if (_aliases.Add(newAlias)) UpdateModificationDetails(clock);
    }

    public void RemoveAlias(string aliasValue, IClock clock)
    {
        var aliasToRemove = new ContactTypeAlias(aliasValue);

        if (_aliases.Remove(aliasToRemove)) UpdateModificationDetails(clock);
    }

    public void UpdateIcons(string? newIconClass, Uri? newIconUrl, IClock clock)
    {
        IconClass = newIconClass;
        IconUrl = newIconUrl;

        EnsureDefaultIcon();
        ValidateInvariants();
        UpdateModificationDetails(clock);
    }

    private void EnsureDefaultIcon()
    {
        if (string.IsNullOrEmpty(IconClass) && IconUrl is null) IconClass = DefaultIconClass;
    }

    private void ValidateInvariants()
    {
        if (string.IsNullOrWhiteSpace(IconClass) && IconUrl is null)
            throw new JobMagnetDomainException(
                $"A {nameof(ContactType)} must have either an {nameof(IconClass)} or an {nameof(IconUrl)}. Both cannot be empty.");
    }
}

public class ContactTypeAliasComparer : IEqualityComparer<ContactTypeAlias>
{
    public bool Equals(ContactTypeAlias? x, ContactTypeAlias? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return string.Equals(x.Alias, y.Alias, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode(ContactTypeAlias obj) => obj.Alias.GetHashCode(StringComparison.OrdinalIgnoreCase);
}