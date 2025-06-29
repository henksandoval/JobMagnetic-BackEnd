using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public record Talent
{
    public const int MaxNameLength = 50;
    public string Description { get; private set; }

    private Talent() { }

    public Talent(string description)
    {
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsLessThanOrEqualTo(description.Length, MaxNameLength);

        Description = description;
    }
}