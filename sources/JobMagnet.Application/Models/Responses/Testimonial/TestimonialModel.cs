using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Models.Responses.Testimonial;

public sealed record TestimonialModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required TestimonialBase TestimonialData { get; init; }
}