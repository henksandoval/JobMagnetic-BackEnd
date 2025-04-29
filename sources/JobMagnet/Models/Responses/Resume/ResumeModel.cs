using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Resume;

public sealed class ResumeModel : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required ResumeBase ResumeData { get; init; }
}