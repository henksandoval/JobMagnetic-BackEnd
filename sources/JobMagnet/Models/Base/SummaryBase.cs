namespace JobMagnet.Models.Base;

public sealed record SummaryBase
{
    public long ProfileId { get; init; }
    public string? Introduction { get; init; }
    public IList<EducationBase> Education { get; init; } = Enumerable.Empty<EducationBase>().ToList();
    public IList<WorkExperienceBase> WorkExperiences { get; init; } = Enumerable.Empty<WorkExperienceBase>().ToList();
}