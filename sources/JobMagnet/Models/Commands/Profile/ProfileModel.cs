using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Profile;

public sealed class ProfileModel : ProfileBase, IIdentifierBase<long>
{
    public long Id { get; init; }
}