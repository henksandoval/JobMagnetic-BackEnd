using System.Text.Json;
using GeminiDotNET;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Infrastructure.ExternalServices.Gemini;
using JobMagnet.Infrastructure.Settings;
using JobMagnet.Shared.Utils;
using Json.Schema;
using Json.Schema.Generation;
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
            .Configure(settings => { settings.FlattenedProfileSchema = LoadAndFlattenProfileSchema(); })
            .ValidateOnStart();

        services
            .AddSingleton<IGeminiClient, GeminiClient>();

        return services;
    }

    private static string LoadAndFlattenProfileSchema()
    {
        var schema = new JsonSchemaBuilder()
            .FromType<ProfileRaw>()
            .Build();

        return JsonSerializer.Serialize(schema);
    }
}

public class GeminiSettingValidation(IConfiguration config) : IValidateOptions<GeminiSettings>
{
    public ValidateOptionsResult Validate(string? name, GeminiSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.ApiKey))
            return ValidateOptionsResult.Fail(
                "Gemini API Key is not configured. Ensure 'Gemini:ApiKey' is set in configuration.");

        if (!Validator.CanBeValidApiKey(settings.ApiKey))
            return ValidateOptionsResult.Fail("Gemini API Key from configuration is invalid.");

        return settings.FlattenedProfileSchema!.IsJsonValid()
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail("Flattened profile schema is not valid JSON.");
    }
}