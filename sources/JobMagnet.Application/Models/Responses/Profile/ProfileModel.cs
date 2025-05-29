using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Models.Responses.Profile;

public sealed record ProfileModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required ProfileBase ProfileData { get; init; }
}