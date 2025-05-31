using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Testimonial;

public sealed record TestimonialModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required TestimonialBase TestimonialData { get; init; }
}