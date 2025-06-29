using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.ProfileHeader;

public sealed record ProfileHeaderResponse : IIdentifierBase<Guid>
{
    public required ResumeBase ResumeData { get; init; }
    public required Guid Id { get; init; }
}