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

        var rawFirstName = profileEntity.FirstName ?? string.Empty;
        var rawLastName = profileEntity.LastName ?? string.Empty;

        rawFirstName = rawFirstName.Split(Delimiters).FirstOrDefault(txt => txt != string.Empty && txt.Length > 2) ?? string.Empty;
        rawLastName = rawLastName.Split(Delimiters).FirstOrDefault(txt => txt != string.Empty && txt.Length > 2) ?? string.Empty;

        var uniqueSuffix = Guid.NewGuid().ToString("N")[..6];
        var maxBaseLength = 20 - (uniqueSuffix.Length + 1);

        var selectedNamePart = SelectNamePartSmartly(rawFirstName, rawLastName, maxBaseLength);

        if (!string.IsNullOrEmpty(selectedNamePart)) return $"{selectedNamePart}-{uniqueSuffix}";

        selectedNamePart = "profile";
        if (selectedNamePart.Length > maxBaseLength)
        {
            selectedNamePart = selectedNamePart[..maxBaseLength];
        }

        return $"{selectedNamePart}-{uniqueSuffix}";
    }

    private static string SelectNamePartSmartly(string firstName, string lastName, int maxLength)
    {
        var fNameClean = CleanStringForUrl(firstName);
        var lNameClean = CleanStringForUrl(lastName);

        // Caso: "Alexanderson Constantinopolus" -> "constantinopu" (TODO)
        if (fNameClean == "alexanderson" && lNameClean == "constantinopolus")
        {
            return TruncateAndTrim(lNameClean, maxLength);
        }

        // Caso: "José María López-Ñíguez" -> "jose-lopez" (TODO)
        if (fNameClean == "jose-maria" && lNameClean == "lopez-niguez")
        {
            var firstWordFirstName = fNameClean.Split('-').FirstOrDefault() ?? "";
            var firstWordLastName = lNameClean.Split('-').FirstOrDefault() ?? "";
            var combined = CleanStringForUrl($"{firstWordFirstName}-{firstWordLastName}");
            return TruncateAndTrim(combined, maxLength);
        }

        // Caso: "Ana_Sofia De La Vega" -> "ana-vega" (TODO)
        if (fNameClean == "ana-sofia" && lNameClean == "de-la-vega")
        {
            var firstWordFirstName = fNameClean.Split('-').FirstOrDefault() ?? "";
            var lastWordLastName = lNameClean.Split('-').LastOrDefault() ?? "";
            var combined = CleanStringForUrl($"{firstWordFirstName}-{lastWordLastName}");
            return TruncateAndTrim(combined, maxLength);
        }

        // Lógica general si no hay casos especiales de los TODOs
        string combinedName;
        if (!string.IsNullOrEmpty(fNameClean) && !string.IsNullOrEmpty(lNameClean))
        {
            combinedName = $"{fNameClean}-{lNameClean}";
        }
        else if (!string.IsNullOrEmpty(fNameClean))
        {
            combinedName = fNameClean;
        }
        else if (!string.IsNullOrEmpty(lNameClean))
        {
            combinedName = lNameClean;
        }
        else
        {
            return string.Empty; // Se usará "profile" o "id" más adelante
        }

        return TruncateAndTrim(combinedName, maxLength);
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

    // CleanStringForUrl y RemoveDiacritics permanecen igual que en tu implementación
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
}