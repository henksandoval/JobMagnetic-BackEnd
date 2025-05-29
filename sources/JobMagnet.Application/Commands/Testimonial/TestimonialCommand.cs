using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Commands.Testimonial;

public record TestimonialCommand
{
    public required TestimonialBase TestimonialData { get; init; }
}