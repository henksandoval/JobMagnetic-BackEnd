using JobMagnet.Application.Contracts.Commands.CareerHistory;
using JobMagnet.Application.Contracts.Responses.CareerHistory;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class CareerHistoryMapper
{
    static CareerHistoryMapper()
    {
        TypeAdapterConfig<CareerHistory, CareerHistoryResponse>
            .NewConfig()
            .Map(dest => dest.CareerHistoryData, src => src);

        TypeAdapterConfig<CareerHistory, CareerHistoryCommand>
            .NewConfig()
            .Map(dest => dest.CareerHistoryData, src => src);
    }
}