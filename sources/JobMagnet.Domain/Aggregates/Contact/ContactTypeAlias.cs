using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Aggregates.Contact;

public class ContactTypeAlias : SoftDeletableEntity<int>
{
    public const int MaxAliasLength = 20;
    public string Alias { get; private set; }
    public int ContactTypeId { get; private set; }
    public bool ContactTypeExist => Id > 0 && !IsDeleted;

    private ContactTypeAlias()
    {
    }

    [SetsRequiredMembers]
    internal ContactTypeAlias(string alias, int contactTypeId, int id = 0)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsNotNullOrEmpty(alias);
        Guard.IsGreaterThanOrEqualTo(contactTypeId, 0);

        Id = id;
        Alias = alias;
        ContactTypeId = contactTypeId;
    }
}