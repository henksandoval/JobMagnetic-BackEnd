namespace JobMagnet.Extensions.SettingSections;

public record GeminiSettings
{
    public string? ApiKey { get; set; }

    public string? FlattenedProfileSchema { get; set; }
}