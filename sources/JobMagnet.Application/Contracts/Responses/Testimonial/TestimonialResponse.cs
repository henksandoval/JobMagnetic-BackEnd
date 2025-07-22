using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Testimonial;

public sealed record TestimonialResponse : IIdentifierBase<Guid>
{
    public required TestimonialBase TestimonialData { get; init; }
    public required Guid Id { get; init; }
}