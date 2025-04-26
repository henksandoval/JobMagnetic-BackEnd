using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Resume;
using Mapster;

namespace JobMagnet.Mappers;

internal static class ResumeMapper
{
    static ResumeMapper()
    {
        TypeAdapterConfig<ResumeUpdateRequest, ResumeEntity>.NewConfig()
            .Ignore(destination => destination.Id);
    }

    internal static ResumeModel ToModel(ResumeEntity entity)
    {
        return entity.Adapt<ResumeModel>();
    }

    internal static ResumeEntity ToEntity(ResumeCreateRequest request)
    {
        return request.Adapt<ResumeEntity>();
    }

    internal static ResumeUpdateRequest ToUpdateRequest(ResumeEntity entity)
    {
        return entity.Adapt<ResumeUpdateRequest>();
    }

    internal static void UpdateEntity(this ResumeEntity entity, ResumeUpdateRequest request)
    {
        request.Adapt(entity);
    }
}