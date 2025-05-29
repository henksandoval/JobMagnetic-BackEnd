using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Models.Responses.Resume;

public sealed record ResumeModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required ResumeBase ResumeData { get; init; }
}