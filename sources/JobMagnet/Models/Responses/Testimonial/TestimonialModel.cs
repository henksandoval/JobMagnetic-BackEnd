using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Testimonial;

public sealed class TestimonialModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required TestimonialBase TestimonialData { get; set; }
}