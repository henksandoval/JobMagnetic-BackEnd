using System.Text.Json;

namespace JobMagnet.Shared.Utils;

public static class StringExtensions
{
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
}