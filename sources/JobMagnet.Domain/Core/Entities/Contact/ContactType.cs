using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities.Contact;

public class ContactType : SoftDeletableEntity<int>
{
    public const int MaxNameLength = 20;
    public const int MaxIconClassLength = 20;
    private const string? DefaultIconClass = "bx bx-link-alt";

    private readonly HashSet<ContactTypeAlias> _aliases = [];
    public string Name { get; private set; }
    public string? IconClass { get; private set; }
    public Uri? IconUrl { get; private set; }
    public virtual IReadOnlyCollection<ContactTypeAlias> Aliases => _aliases;

    private ContactType()
    {
    }

    [SetsRequiredMembers]
    public ContactType(string name, int id = 0, string? iconClass = null, Uri? iconUrl = null)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsNotNullOrEmpty(name);

        Id = id;
        Name = name;

        if (string.IsNullOrEmpty(iconClass) && iconUrl is null) IconClass = DefaultIconClass;

        IconClass = iconClass;
        IconUrl = iconUrl;
    }

    public void AddAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);

        if (_aliases.Any(a => a.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase)))
            throw new JobMagnetDomainException($"The alias ({alias}) already exists.");

        var newAlias = new ContactTypeAlias(alias, Id);
        _aliases.Add(newAlias);
    }

    public void RemoveAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);

        var aliasToRemove = _aliases.FirstOrDefault(a => a.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase));

        aliasToRemove?.Delete();
    }

    public void UpdateIcons(string? newIconClass, Uri? newIconUrl)
    {
        IconClass = newIconClass;
        IconUrl = newIconUrl;

        ValidateInvariants();
    }

    private void ValidateInvariants()
    {
        if (string.IsNullOrWhiteSpace(IconClass) && IconUrl is null)
            throw new JobMagnetDomainException(
                $"A {nameof(ContactType)} must have either an {nameof(IconClass)} or an {nameof(IconUrl)}. Both cannot be empty.");
    }
}