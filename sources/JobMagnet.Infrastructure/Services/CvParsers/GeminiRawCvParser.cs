using System.Text;
using System.Text.Json;
using CSharpFunctionalExtensions;
using GeminiDotNET;
using GeminiDotNET.ClientModels;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Infrastructure.ExternalServices.Gemini;
using JobMagnet.Infrastructure.Services.CvParsers.Exceptions;
using JobMagnet.Infrastructure.Settings;
using JobMagnet.Shared.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JobMagnet.Infrastructure.Services.CvParsers;

public class GeminiCvParser(IGeminiClient geminiClient, IOptions<GeminiSettings> options, ILogger<GeminiCvParser> logger) : IRawCvParser
{
    private readonly GeminiSettings _settings = options.Value;

    public async Task<Maybe<ProfileRaw>> ParseAsync(Stream cvFile)
    {
        if (cvFile.CanSeek) cvFile.Seek(0, SeekOrigin.Begin);

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

            var profileParsed = JsonSerializer.Deserialize<ProfileRaw>(jsonResponse.Value, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
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
            logger.LogWarning(ex,
                "JSON block not found by Regex. Attempting manual cleanup. Original content snippet: {Snippet}",
                originalContent.GetSnippet());
            return AttemptManualCleanup(originalContent);
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Content extracted by Regex was not valid JSON. Original content snippet: {Snippet}",
                originalContent.GetSnippet());
            return AttemptManualCleanup(originalContent);
        }
        catch (TimeoutException ex)
        {
            logger.LogError(ex, "Timeout during JSON extraction. Original content snippet: {Snippet}",
                originalContent.GetSnippet());
            return Maybe<string>.None;
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Invalid input for JSON extraction. Original content snippet: {Snippet}",
                originalContent.GetSnippet());
            return Maybe<string>.None;
        }
    }

    private Maybe<string> AttemptManualCleanup(string? contentToClean)
    {
        if (string.IsNullOrWhiteSpace(contentToClean)) return Maybe<string>.None;

        var jsonOutput = contentToClean;

        if (jsonOutput.StartsWith("```") && jsonOutput.EndsWith("```"))
            jsonOutput = jsonOutput.StartsWith("```json", StringComparison.OrdinalIgnoreCase)
                ? jsonOutput.Substring("```json".Length, jsonOutput.Length - "```json".Length - "```".Length).Trim()
                : jsonOutput.Substring(3, jsonOutput.Length - 6).Trim();

        if (jsonOutput.StartsWith("json", StringComparison.OrdinalIgnoreCase))
        {
            var tempJson = jsonOutput["json".Length..].TrimStart();
            if (tempJson.StartsWith("{") || tempJson.StartsWith("[")) jsonOutput = tempJson;
        }

        jsonOutput = jsonOutput.Trim();

        if (jsonOutput.IsJsonValid())
        {
            logger.LogInformation("Manual JSON cleanup and validation successful. Snippet: {Snippet}",
                jsonOutput.GetSnippet());
            return Maybe<string>.From(jsonOutput);
        }

        logger.LogError("Content after manual cleanup is not valid JSON. Final snippet: {Snippet}",
            jsonOutput.GetSnippet());
        return Maybe<string>.None;
    }

    private async Task<ModelResponse?> CallGeminiServiceAsync(string cvTextContent)
    {
        var fullPrompt = BuildPromptForGemini(cvTextContent);

        var requestBuilder = new ApiRequestBuilder()
            .WithPrompt(fullPrompt)
            .WithDefaultGenerationConfig(0.5f, 8000);

        var request = requestBuilder.Build();
        var response = await geminiClient.GenerateContentAsync(request);
        return response.Value;
    }

    private string BuildPromptForGemini(string cvTextContent)
    {
        var promptBuilder = new StringBuilder();
        promptBuilder.AppendLine("Extract from text to JSON.");
        promptBuilder.AppendLine("Output only the requested JSON; no extra text.");
        promptBuilder.AppendLine("Infer ALL possible information from the 'CV Text' for EVERY field in the 'Target JSON Structure'.");
        promptBuilder.AppendLine("\nResume Content Instructions:");
        promptBuilder.AppendLine(
            "  - For 'ProfileHeader.About': Generate a brief, first-person personal introduction (e.g., 'Hello! I'm [Name], a passionate [Job Title]...'). This should highlight the candidate's professional identity and passion.");
        promptBuilder.AppendLine(
            "  - For 'ProfileHeader.ProfileHeader': Extract or generate a concise list or a few short sentences detailing key professional actions, responsibilities, or contributions from the candidate's experience (e.g., 'Developed and maintained web applications..., Assisted in the development of...'). Focus on what the candidate *did*.");
        promptBuilder.AppendLine(
            "  - For 'ProfileHeader.Overview': Generate a compelling and comprehensive professional overview. This text should summarize the candidate's key skills, overall experience, and suitability for roles, effectively acting as an 'elevator pitch'.");
        promptBuilder.AppendLine("Talents Instructions:");
        promptBuilder.AppendLine(
            "  - From the entire 'CV Text' (including summary, experience, projects, and personal descriptions), identify key professional roles, identities, and core personal attributes or soft skills that define the candidate.");
        promptBuilder.AppendLine(
            "  - Examples of what to look for: 'Developer', 'Photographer', 'Designer', 'Freelancer', 'Creative', 'Problem Solver', 'Team Player', 'Fast Learner', 'Leader', 'Communicator', 'Innovator'.");
        promptBuilder.AppendLine(
            "  - Aim for a concise list of the most prominent and relevant talents, written in the same language as the 'CV Text'.");
        promptBuilder.AppendLine("Skills Instructions:");
        promptBuilder.AppendLine(
            "  - Infer skills for the 'skills' array from the WorkExperience, AcademicDegree, and Courses sections of the 'CV Text'.");
        promptBuilder.AppendLine(
            "  - For each skill, assign a numeric `level` (scale 0-10, where 10 is expert) if the CV provides any indication (e.g., 'advanced' could be 8, 'expert' 10, 'basic' 3).");
        promptBuilder.AppendLine("  - If no clear indication of skill level is found, assign a default `level` of `5`.");
        promptBuilder.AppendLine(
            "Generate a compelling text for the 'ProfileHeader.Overview' field. This text should include a summary of key skills and be based on the 'CV Text', written in the same language as the 'CV Text'.");
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
}