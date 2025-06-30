using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.TalentShowcase;

public sealed record TalentResponse : IIdentifierBase<Guid>
{
    public required TalentBase TalentBase { get; init; }
    public required Guid Id { get; init; }
}