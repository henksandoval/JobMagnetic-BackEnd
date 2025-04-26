namespace JobMagnet.Extensions.SettingSections;

public class SwaggerSettings
{
    public string Url { get; set; } = string.Empty;
    public bool UseUI { get; set; } = false;
    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}