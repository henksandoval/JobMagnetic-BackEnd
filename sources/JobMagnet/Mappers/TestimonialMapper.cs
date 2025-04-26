using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Testimonial;
using Mapster;

namespace JobMagnet.Mappers;

internal static class TestimonialMapper
{
    static TestimonialMapper()
    {
        TypeAdapterConfig<TestimonialUpdateRequest, TestimonialEntity>.NewConfig()
            .Ignore(destination => destination.Id);
    }

    internal static TestimonialModel ToModel(TestimonialEntity entity)
    {
        return entity.Adapt<TestimonialModel>();
    }

    internal static TestimonialEntity ToEntity(TestimonialCreateRequest request)
    {
        return request.Adapt<TestimonialEntity>();
    }

    internal static void UpdateEntity(this TestimonialEntity entity, TestimonialUpdateRequest request)
    {
        request.Adapt(entity);
    }

    internal static TestimonialUpdateRequest ToUpdateRequest(TestimonialEntity entity)
    {
        return entity.Adapt<TestimonialUpdateRequest>();
    }
}