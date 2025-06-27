using JobMagnet.Application.Contracts.Commands.Testimonial;
using JobMagnet.Application.Contracts.Responses.Testimonial;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class TestimonialMapper
{
    static TestimonialMapper()
    {
        TypeAdapterConfig<Testimonial, TestimonialCommand>
            .NewConfig()
            .Map(dest => dest.TestimonialData, src => src);

        TypeAdapterConfig<Testimonial, TestimonialResponse>
            .NewConfig()
            .Map(dest => dest.TestimonialData, src => src);
    }

    public static TestimonialResponse ToModel(this Testimonial entity) => entity.Adapt<TestimonialResponse>();

    public static TestimonialCommand ToUpdateCommand(this Testimonial entity) => entity.Adapt<TestimonialCommand>();
}