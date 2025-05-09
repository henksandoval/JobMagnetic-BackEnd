using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Testimonial;

public record TestimonialCommand
{
    public required TestimonialBase TestimonialData { get; init; }
}