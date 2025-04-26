using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Testimonial;

public sealed class TestimonialModel : TestimonialBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}