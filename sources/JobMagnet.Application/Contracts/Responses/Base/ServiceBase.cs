namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record ServiceBase
{
    public long ProfileId { get; init; }
    public string? Overview { get; init; }

    public IList<ServiceGalleryItemBase> GalleryItems { get; init; } =
        Enumerable.Empty<ServiceGalleryItemBase>().ToList();
}