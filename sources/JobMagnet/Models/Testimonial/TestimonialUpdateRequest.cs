using JobMagnet.Models.Resume;
using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Testimonial;

public class TestimonialUpdateRequest : TestimonialBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}