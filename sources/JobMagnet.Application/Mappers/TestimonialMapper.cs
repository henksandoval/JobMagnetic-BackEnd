using JobMagnet.Application.Contracts.Commands.Testimonial;
using JobMagnet.Application.Contracts.Responses.Testimonial;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class TestimonialMapper
{
    static TestimonialMapper()
    {
        TypeAdapterConfig<TestimonialCommand, Testimonial>
            .NewConfig()
            .Map(dest => dest, src => src.TestimonialData)
            .MapToConstructor(true);

        TypeAdapterConfig<Testimonial, TestimonialCommand>
            .NewConfig()
            .Map(dest => dest.TestimonialData, src => src);

        TypeAdapterConfig<Testimonial, TestimonialResponse>
            .NewConfig()
            .Map(dest => dest.TestimonialData, src => src);
    }

    public static TestimonialResponse ToModel(this Testimonial entity) => entity.Adapt<TestimonialResponse>();

    public static Testimonial ToEntity(this TestimonialCommand command) => command.Adapt<Testimonial>();

    public static void UpdateEntity(this Testimonial entity, TestimonialCommand command)
    {
        command.Adapt(entity);
    }

    public static TestimonialCommand ToUpdateCommand(this Testimonial entity) => entity.Adapt<TestimonialCommand>();
}