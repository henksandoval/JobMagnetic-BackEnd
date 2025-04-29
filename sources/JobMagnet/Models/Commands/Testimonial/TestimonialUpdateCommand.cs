using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Testimonial;

public class TestimonialUpdateCommand : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required TestimonialBase TestimonialData { get; init; }
}