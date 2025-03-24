using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace JobMagnet.Integration.Tests.Extensions;

public static class HttpClientExtensions
{
    public static Task<HttpResponseMessage> PatchAsNewtonsoftJsonAsync<TValue>(
        this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)]
        string? requestUri,
        TValue value,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(client);

        var json = JsonConvert.SerializeObject(value);

        var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.JsonPatch);

        return client.PatchAsync(requestUri, content, cancellationToken);
    }
}