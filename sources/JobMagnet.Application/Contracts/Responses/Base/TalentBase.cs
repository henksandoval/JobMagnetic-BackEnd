namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record TalentBase
{
    public Guid ProfileId { get; init; }
    public string? Description { get; init; }
}