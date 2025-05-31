using System.Text;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using GeminiDotNET;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ClientModels;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using JobMagnet.Domain.Core.Services.CvParser;
using JobMagnet.Domain.Core.Services.CvParser.Interfaces;
using JobMagnet.Infrastructure.Settings;
using JobMagnet.Shared.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JobMagnet.Infrastructure.ExternalServices.CvParsers;

public partial class GeminiCvParser(IOptions<GeminiSettings> options, ILogger<GeminiCvParser> logger) : ICvParser
{
    private readonly GeminiSettings _settings = options.Value;

    public async Task<Maybe<IParsedProfile>> ParseAsync(Stream cvFile)
    {
        if (cvFile.CanSeek)
        {
            cvFile.Seek(0, SeekOrigin.Begin);
        }

        var cvContent = await ReadAndValidateCvFileAsync(cvFile);

        if (cvContent.HasValue) return await ProcessCvWithGeminiAsync(cvContent.Value);

        logger.LogError("CV content is empty or invalid.");
        return Maybe<IParsedProfile>.None;
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

    private async Task<Maybe<IParsedProfile>> ProcessCvWithGeminiAsync(string cvContent)
    {
        logger.LogInformation("Sending CV content to Gemini.");
        try
        {
            var geminiResponse = await CallGeminiServiceAsync(cvContent);

            if (geminiResponse == null || string.IsNullOrWhiteSpace(geminiResponse.Content))
                return Maybe<IParsedProfile>.None;

            var jsonResponse = ParseModelResponseToJson(geminiResponse);

            if (jsonResponse.HasNoValue)
            {
                logger.LogError("Failed to parse Gemini response to JSON.");
                return Maybe<IParsedProfile>.None;
            }

            var profileParsed = JsonConvert.DeserializeObject<ProfileParseDto>(jsonResponse.Value);
            return Maybe<IParsedProfile>.From(profileParsed);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while interacting with Gemini API.");
            return Maybe<IParsedProfile>.None;
        }
    }

    private Maybe<string> ParseModelResponseToJson(ModelResponse modelResponse)
    {
        var jsonToParse = modelResponse.Content;

        jsonToParse = CleanJsonFormat().Replace(jsonToParse!, "$1");

        if (jsonToParse.IsJsonValid())
            return Maybe<string>.From(jsonToParse);

        if (jsonToParse!.StartsWith("```") && jsonToParse.EndsWith("```"))
        {
            jsonToParse = jsonToParse.Substring(3, jsonToParse.Length - 6).Trim();
        }

        if (jsonToParse.StartsWith("json", StringComparison.OrdinalIgnoreCase))
        {
            jsonToParse = jsonToParse[4..].TrimStart();
        }

        jsonToParse = jsonToParse.Trim();

        if (jsonToParse.IsJsonValid()) return Maybe<string>.From(jsonToParse);

        logger.LogError("Content after extraction does not appear to be a valid JSON object or array. Content: {Content}", jsonToParse);
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

    [GeneratedRegex(@"```json\s*([\s\S]*?)\s*```", RegexOptions.IgnoreCase | RegexOptions.Singleline, "en-US")]
    private static partial Regex CleanJsonFormat();
}