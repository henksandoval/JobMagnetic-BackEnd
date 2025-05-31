using System.Text;
using CSharpFunctionalExtensions;
using GeminiDotNET;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ClientModels;
using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Application.UseCases.CvParser.RawDTOs;
using JobMagnet.Infrastructure.ExternalServices.CvParsers.Exceptions;
using JobMagnet.Infrastructure.Settings;
using JobMagnet.Shared.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JobMagnet.Infrastructure.ExternalServices.CvParsers;

public class GeminiCvParser(IOptions<GeminiSettings> options, ILogger<GeminiCvParser> logger) : IRawCvParser
{
    private readonly GeminiSettings _settings = options.Value;

    public async Task<Maybe<ProfileRaw>> ParseAsync(Stream cvFile)
    {
        if (cvFile.CanSeek)
        {
            cvFile.Seek(0, SeekOrigin.Begin);
        }

        var cvContent = await ReadAndValidateCvFileAsync(cvFile);

        if (cvContent.HasValue) return await ProcessCvWithGeminiAsync(cvContent.Value);

        logger.LogError("CV content is empty or invalid.");
        return Maybe<ProfileRaw>.None;
    }

    private async Task<Maybe<string>> ReadAndValidateCvFileAsync(Stream cvFile)
    {
        string cvContent;

        try
        {
            await using var file = cvFile;
            using var reader = new StreamReader(file);
            cvContent = await reader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reading CV file content.");
            return Maybe<string>.None;
        }

        if (string.IsNullOrWhiteSpace(cvContent))
        {
            logger.LogError("Extracted CV content is empty.");
            return Maybe<string>.None;
        }

        logger.LogInformation("CV content extracted, {Length} characters.", cvContent.Length);
        return Maybe<string>.From(cvContent);
    }

    private async Task<Maybe<ProfileRaw>> ProcessCvWithGeminiAsync(string cvContent)
    {
        logger.LogInformation("Sending CV content to Gemini.");
        try
        {
            var geminiResponse = await CallGeminiServiceAsync(cvContent);

            if (geminiResponse == null || string.IsNullOrWhiteSpace(geminiResponse.Content))
                return Maybe<ProfileRaw>.None;

            var jsonResponse = ParseModelResponseToJson(geminiResponse);

            if (jsonResponse.HasNoValue)
            {
                logger.LogError("Failed to parse Gemini response to JSON.");
                return Maybe<ProfileRaw>.None;
            }

            var profileParsed = JsonConvert.DeserializeObject<ProfileRaw>(jsonResponse.Value);
            return Maybe<ProfileRaw>.From(profileParsed);
        }
        catch (Exception ex)
        {
            const string? errorMessage = "An error occurred while interacting with Gemini API.";
            logger.LogError(ex, errorMessage);
            throw new CvParserException(errorMessage, ex);
        }
    }

    private Maybe<string> ParseModelResponseToJson(ModelResponse modelResponse)
    {
        var originalContent = modelResponse.Content;

        try
        {
            var extractedJson = originalContent.ExtractJsonFromMarkdown();
            return Maybe<string>.From(extractedJson);
        }
        catch (FormatException ex)
        {
            logger.LogWarning(ex, "JSON block not found by Regex. Attempting manual cleanup. Original content snippet: {Snippet}", originalContent.GetSnippet());
            return AttemptManualCleanup(originalContent);
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Content extracted by Regex was not valid JSON. Original content snippet: {Snippet}", originalContent.GetSnippet());
            return AttemptManualCleanup(originalContent);
        }
        catch (TimeoutException ex)
        {
            logger.LogError(ex, "Timeout during JSON extraction. Original content snippet: {Snippet}", originalContent.GetSnippet());
            return Maybe<string>.None;
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Invalid input for JSON extraction. Original content snippet: {Snippet}", originalContent.GetSnippet());
            return Maybe<string>.None;
        }
    }

    private Maybe<string> AttemptManualCleanup(string? contentToClean)
    {
        if (string.IsNullOrWhiteSpace(contentToClean))
        {
            return Maybe<string>.None;
        }

        var jsonOutput = contentToClean;

        if (jsonOutput.StartsWith("```") && jsonOutput.EndsWith("```"))
        {
            jsonOutput = jsonOutput.StartsWith("```json", StringComparison.OrdinalIgnoreCase)
                ? jsonOutput.Substring("```json".Length, jsonOutput.Length - "```json".Length - "```".Length).Trim()
                : jsonOutput.Substring(3, jsonOutput.Length - 6).Trim();
        }

        if (jsonOutput.StartsWith("json", StringComparison.OrdinalIgnoreCase))
        {
            var tempJson = jsonOutput["json".Length..].TrimStart();
            if (tempJson.StartsWith("{") || tempJson.StartsWith("[")) {
                jsonOutput = tempJson;
            }
        }

        jsonOutput = jsonOutput.Trim();

        if (jsonOutput.IsJsonValid())
        {
            logger.LogInformation("Manual JSON cleanup and validation successful. Snippet: {Snippet}", jsonOutput.GetSnippet());
            return Maybe<string>.From(jsonOutput);
        }

        logger.LogError("Content after manual cleanup is not valid JSON. Final snippet: {Snippet}", jsonOutput.GetSnippet());
        return Maybe<string>.None;
    }

    private string BuildPromptForGemini(string cvTextContent)
    {
        var promptBuilder = new StringBuilder();
        promptBuilder.AppendLine("Extract from text to JSON.");
        promptBuilder.AppendLine("Output only the requested JSON; no extra text.");
        promptBuilder.AppendLine("Dates: use YYYY-MM or YYYY for partials. If current, endDate is null.");
        promptBuilder.AppendLine("\nCV Text:");
        promptBuilder.AppendLine("```text");
        promptBuilder.AppendLine(cvTextContent);
        promptBuilder.AppendLine("```");
        promptBuilder.AppendLine("\nTarget JSON Structure (flattened):");
        promptBuilder.AppendLine("```json");
        promptBuilder.AppendLine(_settings.FlattenedProfileSchema);
        promptBuilder.AppendLine("```");
        promptBuilder.AppendLine("\nExtracted JSON:");
        return promptBuilder.ToString();
    }

    private async Task<ModelResponse?> CallGeminiServiceAsync(string cvTextContent)
    {
        var generator = new Generator(_settings.ApiKey!);
        var fullPrompt = BuildPromptForGemini(cvTextContent);

        var requestBuilder = new ApiRequestBuilder()
            .WithPrompt(fullPrompt)
            .WithDefaultGenerationConfig(temperature: 0.5f, maxOutputTokens: 2048);

        var request = requestBuilder.Build();

        var response = await generator.GenerateContentAsync(request, ModelVersion.Gemini_20_Flash);
        return response;
    }
}