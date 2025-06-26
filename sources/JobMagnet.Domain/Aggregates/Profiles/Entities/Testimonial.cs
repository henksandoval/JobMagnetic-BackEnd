using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct TestimonialId(Guid Value) : IStronglyTypedId<Guid>;

public class Testimonial : SoftDeletableEntity<TestimonialId>
{
    public string Name { get; private set; }
    public string JobTitle { get; private set; }
    public string? PhotoUrl { get; private set; }
    public string Feedback { get; private set; }
    public ProfileId ProfileId { get; private set; }

    private Testimonial() : base(new TestimonialId(), Guid.Empty)
    {
    }

    [SetsRequiredMembers]
    public Testimonial(string name, string jobTitle, string feedback, string? photoUrl, ProfileId profileId, TestimonialId testimonialId) : base(
        testimonialId, Guid.Empty)
    {
        Guard.IsNotNullOrWhiteSpace(name);
        Guard.IsNotNullOrWhiteSpace(jobTitle);
        Guard.IsNotNullOrWhiteSpace(feedback);

        Name = name;
        JobTitle = jobTitle;
        Feedback = feedback;
        ProfileId = profileId;
        PhotoUrl = photoUrl;
    }
}