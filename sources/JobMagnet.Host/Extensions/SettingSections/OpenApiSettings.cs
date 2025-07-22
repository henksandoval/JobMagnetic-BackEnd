namespace JobMagnet.Host.Extensions.SettingSections;

public class OpenApiSettings
{
    public string Url { get; set; } = string.Empty;
    public bool UseUi { get; set; } = false;
    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}