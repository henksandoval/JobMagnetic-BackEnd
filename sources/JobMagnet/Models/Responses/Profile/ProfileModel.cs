using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Profile;

public sealed class ProfileModel : ProfileBase, IIdentifierBase<long>
{
    public long Id { get; init; }
}