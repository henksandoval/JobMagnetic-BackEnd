using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Testimonial;

public record TestimonialUpdateCommand : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required TestimonialBase TestimonialData { get; init; }
}