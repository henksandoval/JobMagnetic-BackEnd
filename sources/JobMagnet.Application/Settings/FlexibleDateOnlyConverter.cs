using System.Globalization;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

public class FlexibleDateOnlyConverter : JsonConverter<DateOnly?>
{
    private const string Format_YYYY_MM_DD = "yyyy-MM-dd";
    private const string Format_YYYY_MM = "yyyy-MM";
    private const string Format_YYYY = "yyyy";

    public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonToken.String)
        {
            string? dateString = reader.Value?.ToString();
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return null;
            }

            // Intenta parsear como YYYY-MM-DD (fecha completa)
            if (DateOnly.TryParseExact(dateString, Format_YYYY_MM_DD, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly result))
            {
                return result;
            }

            // Intenta parsear como YYYY-MM (asume día 01)
            // Verifica la longitud para evitar que "2015-11-XX" (mal formado) entre aquí si el parseo anterior falló por el día.
            if (dateString.Length == Format_YYYY_MM.Length &&
                DateOnly.TryParseExact(dateString + "-01", Format_YYYY_MM_DD, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }

            // Intenta parsear como YYYY (asume mes 01 y día 01)
            // Verifica la longitud
            if (dateString.Length == Format_YYYY.Length &&
                DateOnly.TryParseExact(dateString + "-01-01", Format_YYYY_MM_DD, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }

            // Si ninguno de los formatos funciona, puedes lanzar una excepción o retornar null
            // dependiendo de cuán estricto quieras ser.
            // Por ahora, lanzamos excepción para señalar un formato inesperado.
            throw new JsonSerializationException($"Could not parse date string '{dateString}' to DateOnly. Expected formats: YYYY-MM-DD, YYYY-MM, or YYYY.");
        }

        throw new JsonSerializationException($"Unexpected token type: {reader.TokenType} when parsing DateOnly.");
    }

    public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
    {
        if (value.HasValue)
        {
            // Al escribir, usa el formato estándar YYYY-MM-DD.
            writer.WriteValue(value.Value.ToString(Format_YYYY_MM_DD, CultureInfo.InvariantCulture));
        }
        else
        {
            writer.WriteNull();
        }
    }
}