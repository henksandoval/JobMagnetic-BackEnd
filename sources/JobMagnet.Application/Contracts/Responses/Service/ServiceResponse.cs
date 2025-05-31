using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Service;

public sealed record ServiceResponse : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required ServiceBase ServiceData { get; init; }
}