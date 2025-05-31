using JobMagnet.Application.Contracts.Commands.Testimonial;
using JobMagnet.Application.Contracts.Responses.Testimonial;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class TestimonialMapper
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

    public static TestimonialModel ToModel(this TestimonialEntity entity)
    {
        return entity.Adapt<TestimonialModel>();
    }

    public static TestimonialEntity ToEntity(this TestimonialCommand command)
    {
        return command.Adapt<TestimonialEntity>();
    }

    public static void UpdateEntity(this TestimonialEntity entity, TestimonialCommand command)
    {
        command.Adapt(entity);
    }

    public static TestimonialCommand ToUpdateCommand(this TestimonialEntity entity)
    {
        return entity.Adapt<TestimonialCommand>();
    }
}