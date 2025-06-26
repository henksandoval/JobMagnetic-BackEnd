using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public class SocialProof
{
    private readonly Profile _profile;
    private IReadOnlyCollection<Testimonial> Testimonials => _profile.Testimonials;

    private SocialProof()
    {
    }

    internal SocialProof(Profile profile)
    {
        Guard.IsNotNull(profile);

        _profile = profile;
    }

    public void AddTestimonial(string name, string jobTitle, string feedback, string? photoUrl = null)
    {
        if (Testimonials.Count >= 20) throw new JobMagnetDomainException("Cannot add more than 20 testimonials.");
        if (Testimonials.Any(t => t.Name == name && t.Feedback == feedback))
            throw new JobMagnetDomainException("This exact testimonial already exists.");

        var testimonial = new Testimonial(name, jobTitle, feedback, photoUrl, _profile.Id);

        _profile.AddTestimonial(testimonial);
    }
}