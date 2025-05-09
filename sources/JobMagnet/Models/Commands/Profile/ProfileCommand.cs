using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Profile;

public sealed record ProfileCommand
{
    public required ProfileBase ProfileData { get; init; }
}