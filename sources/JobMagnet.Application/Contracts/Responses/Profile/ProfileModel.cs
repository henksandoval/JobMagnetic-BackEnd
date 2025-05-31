using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Profile;

public sealed record ProfileModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required ProfileBase ProfileData { get; init; }
}