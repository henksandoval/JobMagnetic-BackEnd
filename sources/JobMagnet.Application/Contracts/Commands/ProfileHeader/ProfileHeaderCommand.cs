using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.ProfileHeader;

public sealed record ProfileHeaderCommand
{
    public required ProfileHeaderBase ProfileHeaderData { get; init; }
}