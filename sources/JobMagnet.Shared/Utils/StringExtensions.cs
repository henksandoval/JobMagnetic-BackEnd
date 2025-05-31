using System.Text.Json;
using System.Text.RegularExpressions;

namespace JobMagnet.Shared.Utils;

public static class StringExtensions
{
    private static readonly Regex JsonBlockRegex = new(
        @"```json\s*([\s\S]*?)\s*```",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(150)
    );

    public static bool IsJsonValid(this string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return false;

        try
        {
            using var jsonDoc = JsonDocument.Parse(json);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    public static string ExtractJsonFromMarkdown(this string? inputText)
    {
        ArgumentNullException.ThrowIfNull(inputText);

        if (string.IsNullOrWhiteSpace(inputText))
            throw new ArgumentException("Input text cannot be empty or whitespace.", nameof(inputText));

        string extractedContent;
        try
        {
            var match = JsonBlockRegex.Match(inputText);

            if (match is { Success: true, Groups.Count: > 1 })
                extractedContent = match.Groups[1].Value;
            else
                throw new FormatException("Markdown text does not contain a recognizable JSON code block.");
        }
        catch (RegexMatchTimeoutException ex)
        {
            throw new TimeoutException("Timeout occurred while trying to extract JSON from Markdown.", ex);
        }

        if (!IsJsonValid(extractedContent))
            throw new JsonException("The content extracted from the Markdown block is not valid JSON.");

        return extractedContent;
    }

    public static string GetSnippet(this string? content, int maxLength = 50)
    {
        if (string.IsNullOrEmpty(content)) return string.Empty;
        return content.Length <= maxLength ? content : content[..maxLength] + "... (truncated)";
    }
}