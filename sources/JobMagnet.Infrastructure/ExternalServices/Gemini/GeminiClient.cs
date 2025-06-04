using CSharpFunctionalExtensions;
using GeminiDotNET;
using GeminiDotNET.ApiModels.ApiRequest;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ClientModels;
using JobMagnet.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JobMagnet.Infrastructure.ExternalServices.Gemini;

public class GeminiClient(IOptions<GeminiSettings> options, ILogger<GeminiClient> logger) : IGeminiClient
{
    private readonly GeminiSettings _settings = options.Value;

    public async Task<Maybe<ModelResponse>> GenerateContentAsync(ApiRequest request, ModelVersion modelVersion)
    {
        var generator = new Generator(_settings.ApiKey!);
        logger.LogInformation("Sending request to Gemini API with model version: {ModelVersion}", modelVersion);
        var response = await generator.GenerateContentAsync(request, modelVersion);

        if (response is not null) return Maybe.From(response);

        logger.LogError("Gemini API response is null.");
        return Maybe<ModelResponse>.None;

    }
}