using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base.Aggregates;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct TestimonialId(Guid Value) : IStronglyTypedId<TestimonialId>;

public class Testimonial : SoftDeletableEntity<TestimonialId>
{
    public string Name { get; private set; }
    public string JobTitle { get; private set; }
    public string? PhotoUrl { get; private set; }
    public string Feedback { get; private set; }
    public ProfileId ProfileId { get; private set; }

    private Testimonial()
    {
    }

    private Testimonial(TestimonialId id, ProfileId profileId, string name, string jobTitle, string feedback, string? photoUrl) :
        base(id)
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

    public static Testimonial CreateInstance(IGuidGenerator guidGenerator, ProfileId profileId, string name, string jobTitle,
        string feedback, string? photoUrl)
    {
        var id = new TestimonialId(guidGenerator.NewGuid());
        return new Testimonial(id, profileId, name, jobTitle, feedback, photoUrl);
    }
}