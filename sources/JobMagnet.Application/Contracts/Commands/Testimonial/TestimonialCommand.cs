using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.Testimonial;

public record TestimonialCommand
{
    public required TestimonialBase TestimonialData { get; init; }
}