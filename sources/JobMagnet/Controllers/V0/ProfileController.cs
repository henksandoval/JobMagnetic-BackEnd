using System.Net.Mime;
using Asp.Versioning;
using GeminiDotNET;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ClientModels;
using JobMagnet.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers.V0;

[ApiVersion("0.1")]
public class ProfileController(ILogger<ProfileController> logger) : BaseController<ProfileController>(logger)
{
    //https://github.com/phanxuanquang/Gemini.NET
    [HttpPost]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    public async Task<IResult> CreateAsync(IFormFile cvFile)
    {
        if (cvFile.Length == 0)
        {
            return Results.BadRequest("File is empty.");
        }

        await using var stream = cvFile.OpenReadStream();
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();
        await TryGeminiAsync(content);

        return Results.Ok();
    }

    private static async Task TryGeminiAsync(string content)
    {
        string apiKey = "AIzaSyCMKo62lm5EGRiLDuA8SjizOhSTdF0G5Xs";

        if (!Validator.CanBeValidApiKey(apiKey))
        {
            Console.WriteLine("Invalid API Key format.");
            return;
        }

        var generator = new Generator(apiKey);

        var request = new ApiRequestBuilder()
            .WithPrompt("Explain the concept of Large Language Models in simple terms.")
            .WithDefaultGenerationConfig(temperature: 0.7f, maxOutputTokens: 512)
            .Build();

        try
        {
            ModelResponse response = await generator.GenerateContentAsync(request, ModelVersion.Gemini_20_Flash);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}