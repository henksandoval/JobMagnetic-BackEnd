using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Resume;

public sealed class ResumeModel : ResumeBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}