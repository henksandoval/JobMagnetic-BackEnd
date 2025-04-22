using System.Collections.Immutable;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record TestimonialCollection
{
    private readonly long _profileId;

    private readonly IList<(string Name, string JobTitle, string PhotoUrl, string Feedback)> _values =
    [
        ("Jane Smith", "Project Manager", "https://randomuser.me/api/portraits/women/28.jpg",
            "Brandon is a talented developer who consistently delivers high-quality work. His ability to understand client needs and translate them into functional designs is impressive."),
        ("Alice Johnson", "Software Engineer", "https://randomuser.me/api/portraits/women/82.jpg",
            "Working with Brandon has been a pleasure. He is always willing to go the extra mile to ensure the project is a success. His technical skills and creativity are top-notch."),
        ("John Smith", "UX Designer", "https://randomuser.me/api/portraits/men/31.jpg",
            "The project was delivered on time and exceeded our expectations. Highly recommend!"),
        ("Michael Brown", "CTO", "https://randomuser.me/api/portraits/men/82.jpg",
            "The team consistently delivered beyond expectations and maintained excellent communication."),
        ("Emily Davis", "Product Owner", "https://randomuser.me/api/portraits/women/11.jpg",
            "Their innovative solutions and commitment to quality have been pivotal in our project’s success, making them an invaluable partner in our journey.")
    ];

    public TestimonialCollection(long profileId = 0)
    {
        _profileId = profileId;
    }

    public ImmutableList<TestimonialEntity> GetTestimonials()
    {
        return _values
            .Select(x => new TestimonialEntity
            {
                Id = 0,
                ProfileId = _profileId,
                Name = x.Name,
                JobTitle = x.JobTitle,
                PhotoUrl = x.PhotoUrl,
                Feedback = x.Feedback,
                AddedAt = DateTime.UtcNow,
                AddedBy = Guid.Empty
            })
            .ToImmutableList();
    }
}