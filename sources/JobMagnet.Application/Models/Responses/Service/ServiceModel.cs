using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Models.Responses.Service;

public sealed record ServiceModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required ServiceBase ServiceData { get; init; }
}