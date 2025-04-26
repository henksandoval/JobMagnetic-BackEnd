using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Testimonial;

public class TestimonialUpdateCommand : TestimonialBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}