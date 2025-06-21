using System.Diagnostics.CodeAnalysis;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities.Contact;

public class ContactTypeAlias : SoftDeletableEntity<int>
{
    public string Alias { get; private set; }
    public int ContactTypeId { get; private set; }
    public virtual ContactType ContactType { get; private set; }

    public bool ContactTypeExist => Id > 0 && !IsDeleted;

    private ContactTypeAlias() {}

    [SetsRequiredMembers]
    internal ContactTypeAlias(string alias, ContactType contactType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias, nameof(alias));
        ArgumentNullException.ThrowIfNull(contactType, nameof(contactType));

        Alias = alias;
        ContactType = contactType;
        ContactTypeId = contactType.Id;
    }
}