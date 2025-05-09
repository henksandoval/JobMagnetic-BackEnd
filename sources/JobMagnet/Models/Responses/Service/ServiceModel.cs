using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Service;

public sealed record ServiceModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required ServiceBase ServiceData { get; init; }
}