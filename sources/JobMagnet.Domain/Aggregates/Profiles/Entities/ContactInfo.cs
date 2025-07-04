using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Shared.Base.Aggregates;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct ContactInfoId(Guid Value) : IStronglyTypedId<ContactInfoId>;

public class ContactInfo : TrackableEntity<ContactInfoId>
{
    public string Value { get; private set; }
    public ContactTypeId ContactTypeId { get; private set; }
    public ProfileHeaderId ProfileHeaderId { get; private set; }
    public virtual ContactType ContactType { get; private set; }

    private ContactInfo() : base() { }

    private ContactInfo(ContactInfoId id, string value, ContactType contactType, ProfileHeaderId profileHeaderId) : base(id)
    {
        Guard.IsNotNullOrEmpty(value);
        Guard.IsNotNull(contactType);

        Id = id;
        Value = value;
        ContactTypeId = contactType.Id;
        ContactType = contactType;
        ProfileHeaderId = profileHeaderId;
    }

    internal static ContactInfo CreateInstance(IGuidGenerator guidGenerator, ProfileHeaderId profileHeaderId, string value,
        ContactType contactType)
    {
        var id = new ContactInfoId(guidGenerator.NewGuid());
        return new ContactInfo(id, value, contactType, profileHeaderId);
    }
}