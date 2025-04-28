using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Testimonial;

public sealed class TestimonialCreateCommand
{
    public required TestimonialBase TestimonialData { get; set; }
}