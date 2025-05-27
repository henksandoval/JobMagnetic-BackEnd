using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Asp.Versioning;
using GeminiDotNET;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ClientModels;
using JobMagnet.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers.V0;

[ApiVersion("0.1")]
public class ProfileController : BaseController<ProfileController>
{
    private readonly IConfiguration _configuration;
    private readonly string _geminiApiKey;
    private readonly string _flattenedJsonSchema;

    public ProfileController(
        ILogger<ProfileController> logger,
        IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment) : base(logger)
    {
        _configuration = configuration;
        _geminiApiKey = GetAndValidateApiKey(configuration);
        _flattenedJsonSchema = LoadAndFlattenSchema(webHostEnvironment);
    }

    private string GetAndValidateApiKey(IConfiguration configuration)
    {
        var apiKey = configuration["GeminiApiKey"] ?? throw new ArgumentNullException(nameof(configuration), "Gemini API Key is not configured.");
        if (string.IsNullOrWhiteSpace(apiKey) || !Validator.CanBeValidApiKey(apiKey))
        {
            Logger.LogError("Gemini API Key is invalid or not configured properly.");
            throw new InvalidOperationException("Gemini API Key is invalid or not configured properly.");
        }
        return apiKey;
    }

    private string LoadAndFlattenSchema(IWebHostEnvironment webHostEnvironment)
    {
        try
        {
            var schemaFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "profileSchema.json");
            if (!System.IO.File.Exists(schemaFilePath))
            {
                Logger.LogError("profileSchema.json not found at {Path}", schemaFilePath);
                throw new FileNotFoundException("profileSchema.json not found. Ensure it exists and 'Copy to Output Directory' is set.", schemaFilePath);
            }
            var indentedJsonSchema = System.IO.File.ReadAllText(schemaFilePath);
            using var jsonDoc = JsonDocument.Parse(indentedJsonSchema);
            return JsonSerializer.Serialize(jsonDoc.RootElement, new JsonSerializerOptions { WriteIndented = false });
        }
        catch (JsonException ex)
        {
            Logger.LogError(ex, "Error parsing profileSchema.json.");
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading or processing profileSchema.json.");
            throw;
        }
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> CreateAsync(IFormFile cvFile)
    {
        var (cvContent, errorResult) = await ReadAndValidateCvFileAsync(cvFile);
        if (errorResult != null)
        {
            return errorResult;
        }

        return await ProcessCvWithGeminiAsync(cvContent!);
    }

    private async Task<(string? CvContent, IResult? ErrorResult)> ReadAndValidateCvFileAsync(IFormFile cvFile)
    {
        if (cvFile.Length == 0)
        {
            Logger.LogWarning("CV file is null or empty.");
            return (null, Results.BadRequest("CV File is required and cannot be empty."));
        }

        Logger.LogInformation("Processing CV file: {FileName}, Size: {Length} bytes", cvFile.FileName, cvFile.Length);

        string cvContent;
        try
        {
            await using var stream = cvFile.OpenReadStream();
            using var reader = new StreamReader(stream);
            cvContent = await reader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error reading CV file content.");
            return (null, Results.Problem("Error reading file.", statusCode: StatusCodes.Status500InternalServerError));
        }

        if (string.IsNullOrWhiteSpace(cvContent))
        {
            Logger.LogWarning("Extracted CV content is empty.");
            return (null, Results.BadRequest("File content is empty after reading."));
        }

        Logger.LogInformation("CV content extracted, {Length} characters.", cvContent.Length);
        return (cvContent, null);
    }

    private async Task<IResult> ProcessCvWithGeminiAsync(string cvContent)
    {
        Logger.LogInformation("Sending CV content to Gemini.");
        try
        {
            var geminiResponse = await CallGeminiServiceAsync(cvContent);

            if (geminiResponse != null && !string.IsNullOrWhiteSpace(geminiResponse.Content))
            {
                Logger.LogInformation("Response received from Gemini.");
                return Results.Ok(geminiResponse.Content);
            }

            Logger.LogWarning("Gemini returned no content or the response was empty.");
            return Results.Problem("Gemini returned no content.", statusCode: StatusCodes.Status500InternalServerError);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while interacting with Gemini API.");
            return Results.Problem($"An error occurred with the AI service: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
        }
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
        promptBuilder.AppendLine(_flattenedJsonSchema);
        promptBuilder.AppendLine("```");
        promptBuilder.AppendLine("\nExtracted JSON:");
        return promptBuilder.ToString();
    }

    private async Task<ModelResponse?> CallGeminiServiceAsync(string cvTextContent)
    {
        var generator = new Generator(_geminiApiKey);
        var fullPrompt = BuildPromptForGemini(cvTextContent);

        Logger.LogInformation("Prompt being sent to Gemini (first 200 chars): {PromptStart}", fullPrompt.Substring(0, Math.Min(fullPrompt.Length, 200)));

        var requestBuilder = new ApiRequestBuilder()
            .WithPrompt(fullPrompt)
            .WithDefaultGenerationConfig(temperature: 0.5f, maxOutputTokens: 2048);

        var request = requestBuilder.Build();

        var response = await generator.GenerateContentAsync(request, ModelVersion.Gemini_20_Flash);
        return response;
    }
}