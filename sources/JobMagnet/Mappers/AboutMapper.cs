using JobMagnet.Entities;
using JobMagnet.Models.About;
using Mapster;

namespace JobMagnet.Mappers;

public static class AboutMapper
{
    static AboutMapper()
    {
        TypeAdapterConfig<AboutUpdateRequest, AboutEntity>.NewConfig()
            .Ignore(destination => destination.Id);
    }

    public static AboutModel ToModel(AboutEntity entity)
    {
        return entity.Adapt<AboutModel>();
    }

    public static AboutEntity ToEntity(AboutCreateRequest request)
    {
        return request.Adapt<AboutEntity>();
    }

    public static void UpdateEntity(this AboutEntity entity, AboutUpdateRequest request)
    {
        request.Adapt(entity);
    }

    public static AboutUpdateRequest ToUpdateRequest(AboutEntity entity)
    {
        return entity.Adapt<AboutUpdateRequest>();
    }
}