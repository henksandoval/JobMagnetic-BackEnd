using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Testimonial;

public sealed class TestimonialModel : TestimonialBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}