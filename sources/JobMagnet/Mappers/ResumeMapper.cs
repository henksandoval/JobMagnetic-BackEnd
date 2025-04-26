using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Resume;
using JobMagnet.Models.Responses.Resume;
using Mapster;

namespace JobMagnet.Mappers;

internal static class ResumeMapper
{
    static ResumeMapper()
    {
        TypeAdapterConfig<ResumeUpdateCommand, ResumeEntity>.NewConfig()
            .Ignore(destination => destination.Id);
    }

    internal static ResumeModel ToModel(ResumeEntity entity)
    {
        return entity.Adapt<ResumeModel>();
    }

    internal static ResumeEntity ToEntity(ResumeCreateCommand command)
    {
        return command.Adapt<ResumeEntity>();
    }

    internal static ResumeUpdateCommand ToUpdateRequest(ResumeEntity entity)
    {
        return entity.Adapt<ResumeUpdateCommand>();
    }

    internal static void UpdateEntity(this ResumeEntity entity, ResumeUpdateCommand command)
    {
        command.Adapt(entity);
    }
}