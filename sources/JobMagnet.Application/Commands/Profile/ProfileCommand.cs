using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Commands.Profile;

public sealed record ProfileCommand
{
    public required ProfileBase ProfileData { get; init; }
}