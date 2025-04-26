using System.Collections.Immutable;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

public record ContactInfoCollection
{
    private readonly long _resumeId;

    private readonly List<(string value, int contactTypeId)> _values =
    [
        ("brandon.johnson@example.com", 1),
        ("+1234567890", 12),
        ("https://linkedin.com/in/brandonjohnson", 6),
        ("https://github.com/brandonjohnson", 7),
        ("https://twitter.com/brandonjohnson", 8),
        ("https://brandonjohnson.dev", 5),
        ("https://instagram.com/brandonjohnson", 10),
        ("https://facebook.com/brandonjohnson", 9),
        ("+1234567890", 2)
    ];

    public ContactInfoCollection(long resumeId)
    {
        _resumeId = resumeId;
    }

    public ImmutableList<ContactInfoEntity> GetContactInfoCollection()
    {
        return _values.Select(x => CreateContactInfoEntity(x.value, x.contactTypeId)).ToImmutableList();
    }

    private ContactInfoEntity CreateContactInfoEntity(string value, int contactTypeId)
    {
        return new ContactInfoEntity
        {
            Id = 0,
            Value = value,
            ContactTypeId = contactTypeId,
            ResumeId = _resumeId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        };
    }
}