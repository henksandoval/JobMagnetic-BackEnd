using System.Reflection;
using CSharpFunctionalExtensions;
using GeminiDotNET.ApiModels.ApiRequest;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ClientModels;
using JobMagnet.Infrastructure.ExternalServices.Gemini;

namespace JobMagnet.Integration.Tests.Mocks;

public class MockGeminiClient : IGeminiClient
{
    public Task<Maybe<ModelResponse>> GenerateContentAsync(ApiRequest request, ModelVersion modelVersion)
    {
        var responseContent = LoadGeminiResponse();
        var response = new ModelResponse { Content = responseContent };
        return Task.FromResult(Maybe.From(response));
    }

    private static string LoadGeminiResponse()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string resourceName = "JobMagnet.Integration.Tests.Mocks.CvParsedResponse.md";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}