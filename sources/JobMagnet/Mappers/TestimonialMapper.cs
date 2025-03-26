using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Testimonial;
using Mapster;

namespace JobMagnet.Mappers;

public static class TestimonialMapper
{
    static TestimonialMapper()
    {
        TypeAdapterConfig<TestimonialUpdateRequest, TestimonialEntity>.NewConfig()
            .Ignore(destination => destination.Id);
    }

    public static TestimonialModel ToModel(TestimonialEntity entity)
    {
        return entity.Adapt<TestimonialModel>();
    }

    public static TestimonialEntity ToEntity(TestimonialCreateRequest request)
    {
        return request.Adapt<TestimonialEntity>();
    }

    public static void UpdateEntity(this TestimonialEntity entity, TestimonialUpdateRequest request)
    {
        request.Adapt(entity);
    }

    public static TestimonialUpdateRequest ToUpdateRequest(TestimonialEntity entity)
    {
        return entity.Adapt<TestimonialUpdateRequest>();
    }
}