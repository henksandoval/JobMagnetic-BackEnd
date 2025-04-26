using JobMagnet.Models.Base;

namespace JobMagnet.Models.Testimonial;

public class TestimonialUpdateRequest : TestimonialBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}