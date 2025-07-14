using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Bogus;

namespace JobMagnet.Shared.Tests.Utils;

public static class TestUtilities
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response) =>
        await response.Content.ReadFromJsonAsync<T>(SerializerOptions);

    public static StringContent SerializeRequestContent(object request)
    {
        var json = JsonSerializer.Serialize(request, SerializerOptions);
        return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
    }

    public static T? OptionalValue<T>(Faker faker, Func<Faker, T> valueGenerator, int probabilityPercentage = 50) =>
        faker.Random.Number(99) < probabilityPercentage ? valueGenerator(faker) : default;
}