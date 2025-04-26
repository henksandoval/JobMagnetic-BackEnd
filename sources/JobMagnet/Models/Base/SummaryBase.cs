namespace JobMagnet.Models.Base;

public abstract class SummaryBase
{
    public required long ProfileId { get; set; }
    public string Introduction { get; set; }
}