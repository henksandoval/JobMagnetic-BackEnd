using System.Text;
using System.Text.RegularExpressions;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Domain.Services;

public interface IProfileIdentifierNameGenerator
{
    string GenerateIdentifierName(ProfileEntity profileEntity);
}

public class ProfileIdentifierNameGenerator : IProfileIdentifierNameGenerator
{
    private static readonly char[] Delimiters = [' ', '-', '_'];

    public string GenerateIdentifierName(ProfileEntity profileEntity)
    {
        ArgumentNullException.ThrowIfNull(profileEntity, nameof(profileEntity));

        var rawFirstName = GetFirstSignificantWord(profileEntity.FirstName);
        var rawLastName = GetFirstSignificantWord(profileEntity.LastName);

        rawFirstName = CleanStringForUrl(rawFirstName);
        rawLastName = CleanStringForUrl(rawLastName);

        var combinedName = CombinedName(rawFirstName, rawLastName);

        var uniqueSuffix = Guid.NewGuid().ToString("N")[..6];
        var maxBaseLength = 20 - (uniqueSuffix.Length + 1);

        var selectedNamePart = TruncateAndTrim(combinedName, maxBaseLength);

        if (!string.IsNullOrEmpty(selectedNamePart)) return $"{selectedNamePart}-{uniqueSuffix}";

        selectedNamePart = "profile";
        return $"{selectedNamePart}-{uniqueSuffix}";
    }

    private static string CombinedName(string rawFirstName, string rawLastName)
    {
        var firstNameHasValue = !string.IsNullOrEmpty(rawFirstName);
        var lastNameHasValue = !string.IsNullOrEmpty(rawLastName);

        return (firstNameHasValue, lastNameHasValue) switch
        {
            (true, true) => $"{rawFirstName}-{rawLastName}",
            (true, false) => rawFirstName,
            (false, true) => rawLastName,
            (false, false) => "profile"
        };
    }

    private static string TruncateAndTrim(string input, int maxLength)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        if (input.Length > maxLength)
        {
            input = input[..maxLength];
        }

        return input.TrimEnd('-');
    }

    private static string CleanStringForUrl(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var result = input.ToLowerInvariant();
        result = Regex.Replace(result, @"[\s_]+", "-");
        result = RemoveDiacritics(result);
        result = Regex.Replace(result, @"[^a-z0-9-]", "");
        result = Regex.Replace(result, @"-+", "-");
        result = result.Trim('-');
        return result;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);
        for (var i = 0; i < normalizedString.Length; i++)
        {
            var c = normalizedString[i];
            var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
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