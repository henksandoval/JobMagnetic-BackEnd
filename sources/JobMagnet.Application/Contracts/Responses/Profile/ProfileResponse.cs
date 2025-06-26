using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Profile;

public sealed record ProfileResponse : IIdentifierBase<Guid>
{
    public required ProfileBase ProfileData { get; init; }
    public required Guid Id { get; init; }
}