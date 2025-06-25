using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities;

public class SocialProof
{
    private readonly ProfileEntity _profile;
    private IReadOnlyCollection<TestimonialEntity> Testimonials => _profile.Testimonials;

    internal SocialProof(ProfileEntity profile)
    {
        Guard.IsNotNull(profile);

        _profile = profile;
    }

    public void AddTestimonial(string name, string jobTitle, string feedback, string? photoUrl = null)
    {
        if (Testimonials.Count >= 20) throw new JobMagnetDomainException("Cannot add more than 20 testimonials.");
        if (Testimonials.Any(t => t.Name == name && t.Feedback == feedback))
            throw new JobMagnetDomainException("This exact testimonial already exists.");

        var testimonial = new TestimonialEntity(name, jobTitle, feedback, photoUrl, _profile.Id);

        _profile.AddTestimonial(testimonial);
    }
}