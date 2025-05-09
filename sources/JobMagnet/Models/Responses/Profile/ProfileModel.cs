using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Profile;

public sealed record ProfileModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required ProfileBase ProfileData { get; init; }
}