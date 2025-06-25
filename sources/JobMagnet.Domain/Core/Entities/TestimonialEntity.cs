using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class TestimonialEntity : SoftDeletableEntity<long>
{
    public string Name { get; private set; }
    public string JobTitle { get; private set; }
    public string? PhotoUrl { get; private set; }
    public string Feedback { get; private set; }
    public long ProfileId { get; private set; }

    private TestimonialEntity()
    {
    }

    [SetsRequiredMembers]
    public TestimonialEntity(string name, string jobTitle, string feedback, string? photoUrl = null, long profileId = 0)
    {
        Guard.IsNotNullOrWhiteSpace(name);
        Guard.IsNotNullOrWhiteSpace(jobTitle);
        Guard.IsNotNullOrWhiteSpace(feedback);
        Guard.IsGreaterThanOrEqualTo(profileId, 0);

        Name = name;
        JobTitle = jobTitle;
        Feedback = feedback;
        ProfileId = profileId;
        PhotoUrl = photoUrl;
    }
}