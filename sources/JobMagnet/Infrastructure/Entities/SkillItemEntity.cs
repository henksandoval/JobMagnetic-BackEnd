using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class SkillItemEntity : TrackableEntity<long>
{
    public string Name { get; set; } // Nombre de la habilidad (ej. "C#", "React")
    public int ProficiencyLevel { get; set; } // Nivel de habilidad (1-10)
    public string Category { get; set; } // Categoría (ej. "Programación", "Diseño")
    public int Rank { get; set; }
    public string IconUrl { get; set; }

    [ForeignKey(nameof(Skill))] public long SkillId { get; set; }

    public virtual SkillEntity Skill { get; set; }
}