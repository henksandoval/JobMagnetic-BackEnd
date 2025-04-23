namespace JobMagnet.Infrastructure.DTO.Profile;

// ReSharper disable once ClassNeverInstantiated.Global
public class ServiceDTO
{
    public required string Overview { get; set; }

    public virtual ICollection<ServiceGalleryItemDTO> GalleryItems { get; set; } =
        new HashSet<ServiceGalleryItemDTO>();

    public virtual ProfileDTO Profile { get; set; }
}