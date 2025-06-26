using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct ContactInfoId(Guid Value) : IStronglyTypedId<Guid>;

public class ContactInfo : SoftDeletableEntity<ContactInfoId>
{
    public string Value { get; private set; }
    public ContactTypeId ContactTypeId { get; private set; }
    public HeadlineId HeadlineId { get; private set; }
    public virtual ContactType ContactType { get; private set; }

    private ContactInfo() : base(new ContactInfoId(), Guid.Empty)
    {
    }

    internal ContactInfo(string value, ContactType contactType, ContactInfoId id, Guid addedBy, HeadlineId headlineId) : base(id, addedBy)
    {
        Guard.IsNotNullOrEmpty(value);
        Guard.IsNotNull(contactType);

        Id = id;
        Value = value;
        ContactTypeId = contactType.Id;
        ContactType = contactType;
        HeadlineId = headlineId;
    }
}