using System.ComponentModel.DataAnnotations;

namespace JobMagnet.Infrastructure.Settings;

public record GeminiSettings
{
    public const string SectionName = "Gemini";

    [Required(ErrorMessage = "ApiKey is required.")]
    public string? ApiKey { get; set; }

    [Required(ErrorMessage = "FlattenedProfileSchema is required.")]
    public string? FlattenedProfileSchema { get; set; }
}