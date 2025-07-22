namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record CareerHistoryBase
{
    public Guid ProfileId { get; init; }
    public string? Introduction { get; init; }
    public IList<AcademicDegreeBase> Education { get; init; } = Enumerable.Empty<AcademicDegreeBase>().ToList();
    public IList<WorkExperienceBase> WorkExperiences { get; init; } = Enumerable.Empty<WorkExperienceBase>().ToList();
}