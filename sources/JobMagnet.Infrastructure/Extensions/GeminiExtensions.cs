using System.Reflection;
using System.Text.Json;
using GeminiDotNET;
using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Infrastructure.ExternalServices.CvParsers;
using JobMagnet.Infrastructure.Settings;
using JobMagnet.Shared.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JobMagnet.Infrastructure.Extensions;

internal static class GeminiExtensions
{
    internal static IServiceCollection AddGemini(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSingleton<IValidateOptions<GeminiSettings>, GeminiSettingValidation>()
            .AddOptions<GeminiSettings>()
            .Bind(configuration.GetSection(GeminiSettings.SectionName))
            .Configure(settings =>
            {
                settings.FlattenedProfileSchema = LoadAndFlattenProfileSchema();
            })
            .ValidateOnStart();

        services.AddSingleton<IRawCvParser, GeminiCvParser>();


        return services;
    }

    private static string LoadAndFlattenProfileSchema()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string resourceName = "JobMagnet.Infrastructure.Schemas.profileSchema.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
        }

        using var reader = new StreamReader(stream);
        var indentedJsonSchema = reader.ReadToEnd();
        try
        {
            using var jsonDoc = JsonDocument.Parse(indentedJsonSchema);
            return JsonSerializer.Serialize(jsonDoc.RootElement, new JsonSerializerOptions { WriteIndented = false });
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Error parsing embedded profileSchema.json: {ex.Message}", ex);
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