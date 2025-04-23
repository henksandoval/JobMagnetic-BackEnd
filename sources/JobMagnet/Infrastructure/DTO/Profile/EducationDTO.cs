namespace JobMagnet.Infrastructure.DTO.Profile;

// ReSharper disable once ClassNeverInstantiated.Global
public class EducationDTO
{
    public string Degree { get; set; } // Grado obtenido (ej. "Bachelor's in Computer Science")
    public string InstitutionName { get; set; }
    public string InstitutionLocation { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } // `null` si sigue estudiando
    public string Description { get; set; } // Detalles adicionales

    public virtual SummaryDTO Summary { get; set; }
}