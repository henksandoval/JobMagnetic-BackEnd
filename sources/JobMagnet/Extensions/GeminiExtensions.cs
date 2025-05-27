using System.Text.Json;
using GeminiDotNET;
using JobMagnet.Domain.Profiles;
using JobMagnet.Extensions.SettingSections;
using JobMagnet.Extensions.Utils;
using JobMagnet.Infrastructure.CvParsers;
using Microsoft.Extensions.Options;

namespace JobMagnet.Extensions;

internal static class GeminiExtensions
{
    internal static IServiceCollection AddGemini(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment)
    {
        services
            .AddSingleton<IValidateOptions<GeminiSettings>, GeminiSettingValidation>()
            .AddOptions<GeminiSettings>()
            .Bind(configuration.GetSection(GeminiSettings.SectionName))
            .Configure<IWebHostEnvironment>((settings, env) =>
            {
                settings.FlattenedProfileSchema = LoadAndFlattenProfileSchema(webHostEnvironment);
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<ICvParser, GeminiCvParser>();


        return services;
    }

    private static string LoadAndFlattenProfileSchema(IWebHostEnvironment webHostEnvironment)
    {
        var schemaFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "profileSchema.json");
        if (!File.Exists(schemaFilePath))
        {
            throw new FileNotFoundException(
                $"profileSchema.json not found at {schemaFilePath}. Ensure it exists and 'Copy to Output Directory' is set.",
                schemaFilePath);
        }

        var indentedJsonSchema = File.ReadAllText(schemaFilePath);
        try
        {
            using var jsonDoc = JsonDocument.Parse(indentedJsonSchema);
            return JsonSerializer.Serialize(jsonDoc.RootElement, new JsonSerializerOptions { WriteIndented = false });
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Error parsing profileSchema.json: {ex.Message}", ex);
        }
    }
}

public class GeminiSettingValidation(IConfiguration config) : IValidateOptions<GeminiSettings>
{
    public ValidateOptionsResult Validate(string? name, GeminiSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.ApiKey))
        {
            return ValidateOptionsResult.Fail(
                "Gemini API Key is not configured. Ensure 'Gemini:ApiKey' or 'GeminiApiKey' is set in configuration.");
        }

        if (!Validator.CanBeValidApiKey(settings.ApiKey))
        {
            return ValidateOptionsResult.Fail("Gemini API Key from configuration is invalid.");
        }

        return settings.FlattenedProfileSchema!.IsJsonValid()
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail("Flattened profile schema is not valid JSON.");
    }
}