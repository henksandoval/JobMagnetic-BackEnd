namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public sealed record ProfileImage
{
    public Uri? Url { get; init; }

    private ProfileImage() { }

    public ProfileImage(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            Url = null;
            return;
        }
        
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            throw new ArgumentException("Invalid profile image URL", nameof(url));
            
        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            throw new ArgumentException("Profile image URL must use HTTP or HTTPS", nameof(url));
            
        Url = uri;
    }

    public static ProfileImage Empty => new((string?)null);
    
    public bool HasImage => Url is not null;
    
    public override string ToString() => Url?.ToString() ?? string.Empty;

    public string GetUrl() =>
        Url is null ? string.Empty : Url.ToString();
}