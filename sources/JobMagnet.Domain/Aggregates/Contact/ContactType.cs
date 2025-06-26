using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Contact;

public readonly record struct ContactTypeId(Guid Value) : IStronglyTypedId<Guid>;

public class ContactType : SoftDeletableEntity<ContactTypeId>
{
    public const int MaxNameLength = 20;
    public const int MaxIconClassLength = 20;
    private const string? DefaultIconClass = "bx bx-link-alt";

    private readonly HashSet<ContactTypeAlias> _aliases = new(new ContactTypeAliasComparer());

    public string Name { get; private set; }
    public string? IconClass { get; private set; }
    public Uri? IconUrl { get; private set; }
    public virtual IReadOnlyCollection<ContactTypeAlias> Aliases => _aliases;

    private ContactType() : base(new ContactTypeId(), Guid.Empty)
    {
    }

    public ContactType(ContactTypeId id, Guid addedBy, string name, string? iconClass = null, Uri? iconUrl = null)
        : base(id, addedBy)
    {
        Guard.IsNotNullOrWhiteSpace(name);

        Name = name;
        IconClass = iconClass;
        IconUrl = iconUrl;

        EnsureDefaultIcon();
        ValidateInvariants();
    }

    public void UpdateName(string newName, Guid modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        Name = newName;
        UpdateModificationDetails(modifiedBy);
    }

    public void AddAlias(string aliasValue, Guid modifiedBy)
    {
        var newAlias = new ContactTypeAlias(aliasValue);

        if (_aliases.Add(newAlias)) UpdateModificationDetails(modifiedBy);
    }

    public void RemoveAlias(string aliasValue, Guid modifiedBy)
    {
        var aliasToRemove = new ContactTypeAlias(aliasValue);

        if (_aliases.Remove(aliasToRemove)) UpdateModificationDetails(modifiedBy);
    }

    public void UpdateIcons(string? newIconClass, Uri? newIconUrl, Guid modifiedBy)
    {
        IconClass = newIconClass;
        IconUrl = newIconUrl;

        EnsureDefaultIcon();
        ValidateInvariants();
        UpdateModificationDetails(modifiedBy);
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