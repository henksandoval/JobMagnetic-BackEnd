using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Base;
using JobMagnet.Models.Commands.Testimonial;
using JobMagnet.Models.Responses.Testimonial;
using Mapster;

namespace JobMagnet.Mappers;

internal static class TestimonialMapper
{
    static TestimonialMapper()
    {
        TypeAdapterConfig<TestimonialCommand, TestimonialEntity>
            .NewConfig()
            .Ignore(destination => destination.Id)
            .Map(dest => dest, src => src.TestimonialData);

        TypeAdapterConfig<TestimonialEntity, TestimonialCommand>
            .NewConfig()
            .Map(dest => dest.TestimonialData, src => src);

        TypeAdapterConfig<TestimonialCommand, TestimonialEntity>
            .NewConfig()
            .Map(dest => dest, src => src.TestimonialData);

        TypeAdapterConfig<TestimonialEntity, TestimonialModel>
            .NewConfig()
            .Map(dest => dest.TestimonialData, src => src);
    }

    internal static TestimonialModel ToModel(this TestimonialEntity entity)
    {
        return entity.Adapt<TestimonialModel>();
    }

    internal static TestimonialEntity ToEntity(this TestimonialCommand command)
    {
        return command.Adapt<TestimonialEntity>();
    }

    internal static void UpdateEntity(this TestimonialEntity entity, TestimonialCommand command)
    {
        command.Adapt(entity);
    }

    internal static TestimonialCommand ToUpdateCommand(this TestimonialEntity entity)
    {
        return entity.Adapt<TestimonialCommand>();
    }
}