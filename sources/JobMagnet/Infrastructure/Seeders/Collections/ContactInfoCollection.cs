using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

public record ContactInfoCollection(long ResumeId = 0)
{
    public readonly IReadOnlyList<ContactInfoEntity> ContactInfo = new List<ContactInfoEntity>
    {
        new()
        {
            Id = 0,
            Value = "brandon.johnson@example.com",
            ContactTypeId = 1,
            ResumeId = ResumeId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Value = "+1234567890",
            ContactTypeId = 12,
            ResumeId = ResumeId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Value = "https://linkedin.com/in/brandonjohnson",
            ContactTypeId = 6,
            ResumeId = ResumeId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Value = "https://github.com/brandonjohnson",
            ContactTypeId = 7,
            ResumeId = ResumeId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Value = "https://twitter.com/brandonjohnson",
            ContactTypeId = 8,
            ResumeId = ResumeId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Value = "https://brandonjohnson.dev",
            ContactTypeId = 5,
            ResumeId = ResumeId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Value = "https://instagram.com/brandonjohnson",
            ContactTypeId = 10,
            ResumeId = ResumeId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Value = "https://facebook.com/brandonjohnson",
            ContactTypeId = 9,
            ResumeId = ResumeId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Value = "+1234567890",
            ContactTypeId = 2,
            ResumeId = ResumeId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        }
    };
}