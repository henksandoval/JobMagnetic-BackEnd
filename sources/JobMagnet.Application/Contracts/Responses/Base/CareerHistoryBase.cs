namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record CareerHistoryBase
{
    public Guid ProfileId { get; init; }
    public string? Introduction { get; init; }
    public IList<EducationBase> Education { get; init; } = Enumerable.Empty<EducationBase>().ToList();
    public IList<WorkExperienceBase> WorkExperiences { get; init; } = Enumerable.Empty<WorkExperienceBase>().ToList();
}