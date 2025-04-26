using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Profile;

public class ProfileUpdateCommand : ProfileBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}