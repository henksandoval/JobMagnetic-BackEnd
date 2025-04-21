using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record TestimonialCollection(long ProfileId = 0)
{
    public readonly IReadOnlyList<TestimonialEntity> Testimonials =
    [
        new()
        {
        Id = 0,
        Name = "Jane Smith",
        JobTitle = "Project Manager",
        PhotoUrl = "https://randomuser.me/api/portraits/women/28.jpg",
        Feedback =
            "Brandon is a talented developer who consistently delivers high-quality work. His ability to understand client needs and translate them into functional designs is impressive.",
        ProfileId = ProfileId,
        AddedAt = DateTime.Now,
        AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Alice Johnson",
            JobTitle = "Software Engineer",
            PhotoUrl = "https://randomuser.me/api/portraits/women/82.jpg",
            Feedback =
                "Working with Brandon has been a pleasure. He is always willing to go the extra mile to ensure the project is a success. His technical skills and creativity are top-notch.",
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "John Smith",
            JobTitle = "UX Designer",
            PhotoUrl = "https://randomuser.me/api/portraits/men/31.jpg",
            Feedback = "The project was delivered on time and exceeded our expectations. Highly recommend!",
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Michael Brown",
            JobTitle = "CTO",
            PhotoUrl = "https://randomuser.me/api/portraits/men/82.jpg",
            Feedback =
                "The team consistently delivered beyond expectations and maintained excellent communication.",
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Emily Davis",
            JobTitle = "Product Owner",
            PhotoUrl = "https://randomuser.me/api/portraits/women/11.jpg",
            Feedback =
                "Their innovative solutions and commitment to quality have been pivotal in our project’s success, making them an invaluable partner in our journey.",
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        }
    ];
}