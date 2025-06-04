using CSharpFunctionalExtensions;
using GeminiDotNET.ApiModels.ApiRequest;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ClientModels;

namespace JobMagnet.Infrastructure.ExternalServices.Gemini;

public interface IGeminiClient
{
    Task<Maybe<ModelResponse>> GenerateContentAsync(ApiRequest request, ModelVersion modelVersion = ModelVersion.Gemini_20_Flash);
}