using System.Text.Json;
using GeminiDotNET;
using JobMagnet.Domain.Profiles;
using JobMagnet.Extensions.SettingSections;
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
            .Configure<GeminiSettings>(configuration.GetSection("Gemini"))
            .PostConfigure<GeminiSettings>(settings =>
            {
                settings.FlattenedProfileSchema = LoadAndFlattenProfileSchema(webHostEnvironment);
            })
            .AddOptions<GeminiSettings>()
            .Validate(settings =>
            {
                if (string.IsNullOrWhiteSpace(settings.ApiKey))
                {
                    ValidateOptionsResult.Fail("Gemini API Key is not configured.");
                    return false;
                }
                if (!Validator.CanBeValidApiKey(settings.ApiKey))
                {
                    ValidateOptionsResult.Fail("Gemini API Key from configuration is invalid.");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(settings.FlattenedProfileSchema))
                {
                     ValidateOptionsResult.Fail("Flattened profile schema could not be loaded or is empty.");
                    return false;
                }
                return true;
            })
            .ValidateOnStart();

        services.AddSingleton<ICvParser, GeminiCvParser>();


        return services;
    }

    private static string LoadAndFlattenProfileSchema(IWebHostEnvironment webHostEnvironment)
    {
        var schemaFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "profileSchema.json");
        if (!File.Exists(schemaFilePath))
        {
            throw new FileNotFoundException($"profileSchema.json not found at {schemaFilePath}. Ensure it exists and 'Copy to Output Directory' is set.", schemaFilePath);
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