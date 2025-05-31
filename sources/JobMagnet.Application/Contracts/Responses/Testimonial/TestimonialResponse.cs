using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Testimonial;

public sealed record TestimonialResponse : IIdentifierBase<long>
{
    public required TestimonialBase TestimonialData { get; init; }
    public required long Id { get; init; }
}