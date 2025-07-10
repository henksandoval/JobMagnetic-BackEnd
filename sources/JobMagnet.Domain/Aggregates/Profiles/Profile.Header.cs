using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles;

public partial class Profile
{
    public void AddHeader(IGuidGenerator guidGenerator, string title, string suffix, string jobTitle, string about, string summary, string overview, string address)
    {
        var header = ProfileHeader.CreateInstance(
            guidGenerator,
            Id,
            title,
            suffix,
            jobTitle,
            about,
            summary,
            overview,
            address);

        Header = header;
    }

    public void AddContactInfo(IGuidGenerator guidGenerator, string value, ContactType contactType)
    {
        if (!HaveHeader)
            throw new JobMagnetDomainException($"The profile {Id} does not have a header.");

        Header!.AddContactInfo(guidGenerator, value, contactType);
    }

    public void UpdateHeader(
        string title,
        string suffix,
        string jobTitle,
        string about,
        string summary,
        string overview,
        string address)
    {
        if (HaveHeader)
            Header!.UpdateGeneralInfo(title, suffix, jobTitle, about, summary, overview, address);
    }

    public void RemoveHeader(IClock clock)
    {
        if (!HaveHeader)
            throw new JobMagnetDomainException($"The profile {Id} does not have a header.");

        Header!.MarkAsDeleted(clock);
    }
}