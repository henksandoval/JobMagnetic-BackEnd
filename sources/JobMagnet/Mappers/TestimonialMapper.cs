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
        TypeAdapterConfig<TestimonialUpdateCommand, TestimonialEntity>
            .NewConfig()
            .Ignore(destination => destination.Id)
            .Map(dest => dest, src => src.TestimonialData);

        TypeAdapterConfig<TestimonialEntity, TestimonialUpdateCommand>
            .NewConfig()
            .Map(dest => dest.TestimonialData, src => src);

        TypeAdapterConfig<TestimonialCreateCommand, TestimonialEntity>
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

    internal static TestimonialEntity ToEntity(this TestimonialCreateCommand command)
    {
        return command.Adapt<TestimonialEntity>();
    }

    internal static void UpdateEntity(this TestimonialEntity entity, TestimonialUpdateCommand command)
    {
        command.Adapt(entity);
    }

    internal static TestimonialUpdateCommand ToUpdateRequest(this TestimonialEntity entity)
    {
        return entity.Adapt<TestimonialUpdateCommand>();
    }
}