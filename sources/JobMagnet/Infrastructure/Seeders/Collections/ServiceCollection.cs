using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record ServiceCollection(long ServiceId = 0)
{
    public readonly IReadOnlyList<ServiceGalleryItemEntity> ServicesGallery =
    [
        new()
        {
            Id = 0,
            Position = 1,
            Title = "Web Development",
            Description = "Building responsive and user-friendly websites.",
            UrlLink = "https://example.com/web-development",
            UrlImage = "https://cdn.pixabay.com/photo/2024/08/06/10/43/wine-8949009_1280.jpg",
            UrlVideo = "https://example.com/video1.mp4",
            Type = "image",
            ServiceId = ServiceId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Position = 2,
            Title = "UI/UX Design",
            Description = "Creating intuitive and engaging user interfaces.",
            UrlLink = "https://example.com/ui-ux-design",
            UrlImage = "https://cdn.pixabay.com/photo/2023/08/11/08/29/highland-cattle-8183107_1280.jpg",
            UrlVideo = "https://example.com/video2.mp4",
            Type = "image",
            ServiceId = ServiceId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Position = 3,
            Title = "Web and Brand Graphic Design",
            Description = "Creating intuitive and engaging user interfaces.",
            UrlLink = "https://example.com/ux-design",
            UrlImage = "https://cdn.pixabay.com/photo/2024/02/20/13/21/mountains-8585535_1280.jpg",
            UrlVideo = "https://example.com/video3.mp4",
            Type = "image",
            ServiceId = ServiceId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Position = 4,
            Title = "SEO Consulting",
            Description = "Creating intuitive and engaging user interfaces.",
            UrlLink = "https://example.com/ui-ux-design2",
            UrlImage = "https://cdn.pixabay.com/photo/2024/01/25/10/50/mosque-8531576_1280.jpg",
            UrlVideo = "https://example.com/video4.mp4",
            Type = "image",
            ServiceId = ServiceId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        }
    ];
}