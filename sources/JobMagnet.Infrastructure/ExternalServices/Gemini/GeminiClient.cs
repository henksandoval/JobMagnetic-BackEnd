using CSharpFunctionalExtensions;
using GeminiDotNET;
using GeminiDotNET.ApiModels.ApiRequest;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ClientModels;
using JobMagnet.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace JobMagnet.Infrastructure.ExternalServices.Gemini;

public class GeminiClient(IOptions<GeminiSettings> options) : IGeminiClient
{
    private readonly GeminiSettings _settings = options.Value;

    public async Task<Maybe<ModelResponse>> GenerateContentAsync(ApiRequest request, ModelVersion modelVersion)
    {
        var generator = new Generator(_settings.ApiKey!);
        var response = await generator.GenerateContentAsync(request, ModelVersion.Gemini_20_Flash);
        return Maybe.From(response);
    }
}