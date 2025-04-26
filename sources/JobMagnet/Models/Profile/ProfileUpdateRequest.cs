using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Profile;

public class ProfileUpdateRequest : ProfileBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}