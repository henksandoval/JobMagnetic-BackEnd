using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class EducationEntity : SoftDeletableEntity<long>
{
    public string Degree { get; set; } // Grado obtenido (ej. "Bachelor's in Computer Science")
    public string InstitutionName { get; set; }
    public string InstitutionLocation { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } // `null` si sigue estudiando
    public string Description { get; set; } // Detalles adicionales

    [ForeignKey(nameof(Summary))]
    public long SummaryId { get; set; }
    public virtual SummaryEntity Summary { get; set; }
}