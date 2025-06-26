using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Portfolio;

public sealed record ProjectResponse : IIdentifierBase<long>
{
    public required ProjectBase ProjectData { get; init; }
    public required long Id { get; init; }
}