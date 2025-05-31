using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Resume;

public sealed record ResumeModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required ResumeBase ResumeData { get; init; }
}