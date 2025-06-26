using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Domain.Services;

public interface IProfileSlugGenerator
{
    string GenerateProfileSlug(Profile profile);
}

public partial class ProfileSlugGenerator : IProfileSlugGenerator
{
    private static readonly char[] Delimiters = [' ', '-', '_'];

    public string GenerateProfileSlug(Profile profile)
    {
        ArgumentNullException.ThrowIfNull(profile, nameof(profile));

        var rawFirstName = GetFirstSignificantWord(profile.FirstName);
        var rawLastName = GetFirstSignificantWord(profile.LastName);

        rawFirstName = CleanStringForUrl(rawFirstName);
        rawLastName = CleanStringForUrl(rawLastName);

        var initialSlug = GenerateSlug(rawFirstName, rawLastName);

        var uniqueSuffix = Guid.NewGuid().ToString("N")[..6];
        var maxBaseLength = VanityUrl.MaxNameLength - (uniqueSuffix.Length + 1);

        var selectedNamePart = TruncateAndTrim(initialSlug, maxBaseLength);

        return $"{selectedNamePart}-{uniqueSuffix}";
    }

    [GeneratedRegex(@"[\s_]+")]
    private static partial Regex ConsecutiveWhitespaceOrUnderscoresRegex();

    [GeneratedRegex("[^a-z0-9-]")]
    private static partial Regex FindInvalidUrlSlugCharactersRegex();

    [GeneratedRegex("-+")]
    private static partial Regex MultipleDashRegex();

    private static string GenerateSlug(string rawFirstName, string rawLastName)
    {
        var firstNameHasValue = !string.IsNullOrEmpty(rawFirstName);
        var lastNameHasValue = !string.IsNullOrEmpty(rawLastName);

        return (firstNameHasValue, lastNameHasValue) switch
        {
            (true, true) => $"{rawFirstName}-{rawLastName}",
            (true, false) => rawFirstName,
            (false, true) => rawLastName,
            (false, false) => VanityUrl.DefaultSlug
        };
    }

    private static string TruncateAndTrim(string input, int maxLength)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        if (input.Length > maxLength) input = input[..maxLength];

        return input.TrimEnd('-');
    }

    private static string CleanStringForUrl(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var result = input.ToLowerInvariant();
        result = ConsecutiveWhitespaceOrUnderscoresRegex().Replace(result, "-");
        result = RemoveDiacritics(result);
        result = FindInvalidUrlSlugCharactersRegex().Replace(result, "");
        result = MultipleDashRegex().Replace(result, "-");
        result = result.Trim('-');
        return result;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(normalizedString.Length);
        foreach (var character in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark) stringBuilder.Append(character);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    private static string GetFirstSignificantWord(string? word)
    {
        if (string.IsNullOrWhiteSpace(word)) return string.Empty;

        return word.Split(Delimiters)
                   .FirstOrDefault(text => !string.IsNullOrEmpty(text) && text.Length > 2)
               ?? string.Empty;
    }
}