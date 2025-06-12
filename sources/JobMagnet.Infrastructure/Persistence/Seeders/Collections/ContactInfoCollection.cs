using System.Collections.Immutable;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record ContactInfoCollection
{
    private readonly long _resumeId;

    private readonly List<(string value, string contactType)> _values =
    [
        ("brandon.johnson@example.com", "Email"),
        ("+1234567890", "Mobile Phone"),
        ("https://linkedin.com/in/brandonjohnson", "LinkedIn"),
        ("https://github.com/brandonjohnson", "GitHub"),
        ("https://twitter.com/brandonjohnson", "Twitter"),
        ("https://brandonjohnson.dev", "Web site"),
        ("https://instagram.com/brandonjohnson", "Instagram"),
        ("https://facebook.com/brandonjohnson", "Facebook"),
        ("+9876543210", "Mobile Phone")
    ];
}