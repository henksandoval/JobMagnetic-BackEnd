using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Service;

public sealed record ServiceResponse : IIdentifierBase<long>
{
    public required ServiceBase ServiceData { get; init; }
    public required long Id { get; init; }
}