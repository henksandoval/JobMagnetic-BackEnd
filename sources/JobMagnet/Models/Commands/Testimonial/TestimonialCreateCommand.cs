using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Testimonial;

public sealed record TestimonialCreateCommand
{
    public required TestimonialBase TestimonialData { get; init; }
}