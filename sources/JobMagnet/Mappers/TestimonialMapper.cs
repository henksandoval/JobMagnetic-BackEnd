using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Testimonial;
using Mapster;

namespace JobMagnet.Mappers;

internal static class TestimonialMapper
{
    static TestimonialMapper()
    {
        TypeAdapterConfig<TestimonialUpdateCommand, TestimonialEntity>.NewConfig()
            .Ignore(destination => destination.Id);
    }

    internal static TestimonialModel ToModel(TestimonialEntity entity)
    {
        return entity.Adapt<TestimonialModel>();
    }

    internal static TestimonialEntity ToEntity(TestimonialCreateCommand command)
    {
        return command.Adapt<TestimonialEntity>();
    }

    internal static void UpdateEntity(this TestimonialEntity entity, TestimonialUpdateCommand command)
    {
        command.Adapt(entity);
    }

    internal static TestimonialUpdateCommand ToUpdateRequest(TestimonialEntity entity)
    {
        return entity.Adapt<TestimonialUpdateCommand>();
    }
}