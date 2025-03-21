using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace JobMagnet.Integration.Tests.Utils;

public static class TestUtilities
{
    public static async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseContent);
    }

    public static StringContent SerializeRequestContent(object request)
    {
        var json = JsonConvert.SerializeObject(request);
        return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}