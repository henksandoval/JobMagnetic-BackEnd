using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Portfolio;

public sealed record ProjectResponse : IIdentifierBase<Guid>
{
    public required ProjectBase ProjectData { get; init; }
    public required Guid Id { get; init; }
}