using JobMagnet.Application.Contracts.Commands.CareerHistory;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.CareerHistory;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class CareerHistoryMapper
{
    static CareerHistoryMapper()
    {
        TypeAdapterConfig<WorkExperience, WorkExperienceBase>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value);

        TypeAdapterConfig<AcademicDegree, AcademicDegreeBase>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value);

        TypeAdapterConfig<CareerHistory, CareerHistoryBase>
            .NewConfig()
            .Map(dest => dest.Education, src => src.AcademicDegree.Adapt<List<AcademicDegreeBase>>())
            .Map(dest => dest.ProfileId, src => src.ProfileId.Value);

        TypeAdapterConfig<CareerHistory, CareerHistoryResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.CareerHistoryData, src => src);

        TypeAdapterConfig<CareerHistory, CareerHistoryCommand>
            .NewConfig()
            .Map(dest => dest.CareerHistoryData, src => src);
    }

    public static CareerHistoryResponse ToModel(this CareerHistory careerHistory) => careerHistory.Adapt<CareerHistoryResponse>();
}