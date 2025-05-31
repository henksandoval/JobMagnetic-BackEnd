using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Profile;

public sealed record ProfileResponse : IIdentifierBase<long>
{
    public required ProfileBase ProfileData { get; init; }
    public required long Id { get; init; }
}