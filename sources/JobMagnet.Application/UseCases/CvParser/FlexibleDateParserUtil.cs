using System.Globalization;

namespace JobMagnet.Application.UseCases.CvParser;

public static class FlexibleDateParserUtil
{
    private static readonly string[] DateFormats =
    [
        "yyyy-MM-dd", "M/dd/yyyy", "MM/dd/yyyy", "M/d/yyyy", "MM/d/yyyy", "dd/MM/yyyy", "MMMM d, yyyy",
        "yyyy-MM", "MM/yyyy",
        "yyyy"
    ];

    public static DateOnly? ParseFlexibleDate(string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
        {
            return null;
        }

        // Intenta parsear formatos completos primero
        if (DateOnly.TryParseExact(dateString, DateFormats.Take(7).ToArray(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            return parsedDate;
        }

        // Intenta parsear Mes-Año
        foreach (var format in DateFormats.Skip(7).Take(2)) // "yyyy-MM", "MM/yyyy"
        {
            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtMonthYear))
            {
                return DateOnly.FromDateTime(dtMonthYear); // Por defecto será el día 1
            }
        }

        // Intenta parsear solo Año
        if (DateTime.TryParseExact(dateString, DateFormats.Last(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtYear)) // "yyyy"
        {
            return DateOnly.FromDateTime(dtYear); // Por defecto será 1 de Enero
        }

        // Considerar si "MMMM d, yyyy" necesita una cultura específica si no es en inglés
        // Ejemplo con cultura específica (ej. inglés para "July 15, 1990"):
        if (DateOnly.TryParseExact(dateString, "MMMM d, yyyy", CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out var parsedDateEn))
        {
            return parsedDateEn;
        }


        return null; // No se pudo parsear
    }
}