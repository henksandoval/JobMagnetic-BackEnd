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
            .AddOptions<GeminiSettings>()
            .Bind(configuration.GetSection(GeminiSettings.SectionName))
            .Configure(settings => { settings.FlattenedProfileSchema = LoadAndFlattenProfileSchema(); })
            .Validate(settings =>
            {
                if (string.IsNullOrWhiteSpace(settings.ApiKey) || !Validator.CanBeValidApiKey(settings.ApiKey))
                    return false;

                return settings.FlattenedProfileSchema!.IsJsonValid();
            }, "Invalid Gemini settings configuration")
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