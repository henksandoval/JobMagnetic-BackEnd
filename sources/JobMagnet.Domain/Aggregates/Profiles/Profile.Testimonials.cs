using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles;

public partial class Profile
{
    private void AddTestimonialToCollection(IGuidGenerator guidGenerator, string name, string jobTitle, string feedback, string? photoUrl = null)
    {
        if (Testimonials.Count >= 20) throw new JobMagnetDomainException("Cannot add more than 20 testimonials.");
        if (Testimonials.Any(t => t.Name == name && t.Feedback == feedback))
            throw new JobMagnetDomainException("This exact testimonial already exists.");

        var testimonial = Testimonial.CreateInstance(guidGenerator, Id, name, jobTitle, feedback, photoUrl);

        _testimonials.Add(testimonial);
    }
}