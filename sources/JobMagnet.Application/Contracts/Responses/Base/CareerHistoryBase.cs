namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record CareerHistoryBase
{
    public Guid ProfileId { get; init; }
    public string? Introduction { get; init; }
    public IList<QualificationBase> Education { get; init; } = Enumerable.Empty<QualificationBase>().ToList();
    public IList<WorkExperienceBase> WorkExperiences { get; init; } = Enumerable.Empty<WorkExperienceBase>().ToList();
}