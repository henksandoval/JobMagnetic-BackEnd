using System.Diagnostics.CodeAnalysis;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities.Contact;

public class ContactType : SoftDeletableEntity<int>
{
    public string Name { get; private set; }
    public string? IconClass { get; private set; }
    public string? IconUrl { get; private set; }
    public virtual IReadOnlyCollection<ContactTypeAlias> Aliases => _aliases.AsReadOnly();

    private readonly List<ContactTypeAlias> _aliases = [];

    private ContactType()
    {
    }

    [SetsRequiredMembers]
    public ContactType(int id, string name, string? iconClass = null, Uri? iconUrl = null)
    {
        ArgumentNullException.ThrowIfNull(name);

        Id = id;
        Name = name;
        IconClass = iconClass;
        IconUrl = iconUrl?.AbsoluteUri;
        ValidateInvariants();
    }

    [SetsRequiredMembers]
    public ContactType(string? name)
    {
        Name = name ?? string.Empty;
    }

    public void AddAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);

        if (_aliases.Any(a => a.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase)))
        {
            throw new JobMagnetDomainException($"The alias ({alias}) already exists.");
        }

        var newAlias = new ContactTypeAlias(alias, this);
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
        IconUrl = newIconUrl?.AbsoluteUri;

        ValidateInvariants();
    }

    private void ValidateInvariants()
    {
        if (string.IsNullOrWhiteSpace(IconClass) && string.IsNullOrWhiteSpace(IconUrl))
        {
            throw new JobMagnetDomainException(
                $"A {nameof(ContactType)} must have either an {nameof(IconClass)} or an {nameof(IconUrl)}. Both cannot be empty.");
        }
    }

    public void SetDefaultIcon()
    {
        IconClass = "bx bx-link-alt";
        IconUrl = null;
    }
}