using CSharpFunctionalExtensions;
using GeminiDotNET.ApiModels.ApiRequest;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ClientModels;
using JobMagnet.Infrastructure.ExternalServices.Gemini;
using Microsoft.AspNetCore.Http;

namespace JobMagnet.Integration.Tests.Mocks;

public class MockGeminiClient(IHttpContextAccessor httpContextAccessor) : IGeminiClient
{
    public Task<Maybe<ModelResponse>> GenerateContentAsync(ApiRequest request, ModelVersion modelVersion)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var resourceName = httpContext?.Request.Headers["X-Test-Resource"][0] ??
                           throw new InvalidOperationException("X-Test-Resource header is missing in the request.");

        var content = LoadGeminiResponse(resourceName);
        return Task.FromResult(Maybe.From(new ModelResponse { Content = content }));
    }

    private static string LoadGeminiResponse(string resourceName)
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, resourceName);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"File not found at path: '{fullPath}'");
        }

        return File.ReadAllText(fullPath);
    }
}