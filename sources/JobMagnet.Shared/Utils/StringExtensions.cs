using System.Globalization;
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

    /// <summary>
    /// Parses a string to a <see cref="DateOnly"/>.
    /// Returns null if the input string is null, empty, or whitespace.
    /// Throws a <see cref="FormatException"/> if the string is not empty but cannot be parsed into a known date format.
    /// </summary>
    /// <param name="dateString">The date string to parse.</param>
    /// <returns>A <see cref="DateOnly"/> if parsing is successful and input is not empty/whitespace, otherwise null.
    /// </returns>
    /// <exception cref="FormatException">Thrown if the dateString is not empty/whitespace and not in a recognized format.</exception>
    public static DateOnly? ParseToDateOnly(this string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
        {
            return null;
        }

        string[] fullDateFormats =
        [
            "yyyy-MM-dd",
            "M/dd/yyyy", "MM/dd/yyyy", "M/d/yyyy", "MM/d/yyyy",
            "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy"
        ];

        string[] partialDateFormats =
        [
            "yyyy-MM",
            "MM/yyyy",
            "yyyy"
        ];

        if (TryParseExact(dateString, fullDateFormats, out var parsedDate))
        {
            return parsedDate;
        }

        if (DateTime.TryParseExact(dateString, partialDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var fullDateTime))
        {
            return DateOnly.FromDateTime(fullDateTime);
        }

        throw new FormatException("The provided date string is not in a recognized format.");
    }

    private static bool TryParseExact(string dateString, string[] fullDateFormats, out DateOnly parsedDate)
    {
        return DateOnly.TryParseExact(dateString, fullDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None,
                   out parsedDate) ||
               DateOnly.TryParseExact(dateString, "MMMM d, yyyy", CultureInfo.GetCultureInfo("en-US"),
                   DateTimeStyles.None,
                   out parsedDate);
    }
}