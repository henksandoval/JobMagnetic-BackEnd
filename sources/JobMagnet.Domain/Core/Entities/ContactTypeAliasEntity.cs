using System.Diagnostics.CodeAnalysis;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class ContactTypeAliasEntity : SoftDeletableEntity<int>
{
    public string Alias { get; private set; }
    public int ContactTypeId { get; private set; }
    public virtual ContactTypeEntity ContactType { get; private set; }

    public bool ContactTypeExist => Id > 0 && !IsDeleted;

    private ContactTypeAliasEntity() {}

    [SetsRequiredMembers]
    internal ContactTypeAliasEntity(string alias, ContactTypeEntity contactType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias, nameof(alias));
        ArgumentNullException.ThrowIfNull(contactType, nameof(contactType));

        Alias = alias;
        ContactType = contactType;
        ContactTypeId = contactType.Id;
    }
}