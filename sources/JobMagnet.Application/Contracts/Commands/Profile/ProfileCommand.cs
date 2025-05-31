using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.Profile;

public sealed record ProfileCommand
{
    public required ProfileBase ProfileData { get; init; }
}