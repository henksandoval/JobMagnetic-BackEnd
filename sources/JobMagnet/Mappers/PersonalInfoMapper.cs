using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Resume;
using Mapster;

namespace JobMagnet.Mappers;

public static class PersonalInfoMapper
{
    static PersonalInfoMapper()
    {
        TypeAdapterConfig<ResumeUpdateRequest, ResumeEntity>.NewConfig()
            .Ignore(destination => destination.Id);
    }

    public static ResumeModel ToModel(ResumeEntity entity)
    {
        return entity.Adapt<ResumeModel>();
    }

    public static ResumeEntity ToEntity(ResumeCreateRequest request)
    {
        return request.Adapt<ResumeEntity>();
    }

    public static void UpdateEntity(this ResumeEntity entity, ResumeUpdateRequest request)
    {
        request.Adapt(entity);
    }

    public static ResumeUpdateRequest ToUpdateRequest(ResumeEntity entity)
    {
        return entity.Adapt<ResumeUpdateRequest>();
    }
}