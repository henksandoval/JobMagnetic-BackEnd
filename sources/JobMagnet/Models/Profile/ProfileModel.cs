using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Profile;

public sealed class ProfileModel : ProfileBase, IIdentifierBase<long>
{
    public long Id { get; init; }
}