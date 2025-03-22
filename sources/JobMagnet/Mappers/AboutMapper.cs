using JobMagnet.Entities;
using JobMagnet.Models;
using Mapster;

namespace JobMagnet.Mappers;

public static class AboutMapper
{
    public static AboutModel ToModel(AboutEntity entity)
    {
        return entity.Adapt<AboutModel>();
    }

    public static AboutEntity ToEntity(AboutCreateRequest request)
    {
        return request.Adapt<AboutEntity>();
    }
}