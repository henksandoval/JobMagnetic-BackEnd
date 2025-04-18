namespace JobMagnet.Models.Portfolio;

public abstract class PortfolioBase
{
    public required long ProfileId { get; set; }
    public int Position { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string UrlLink { get; set; }
    public string UrlImage { get; set; }
    public string UrlVideo { get; set; }
    public string Type { get; set; }
}