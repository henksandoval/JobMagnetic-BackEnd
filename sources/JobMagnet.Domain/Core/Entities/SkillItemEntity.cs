using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class SkillItemEntity : TrackableEntity<long>
{
    public string Name { get; set; } // Nombre de la habilidad (ej. "C#", "React")
    public ushort ProficiencyLevel { get; set; } // Nivel de habilidad (1-10)
    public string Category { get; set; } // Categoría (ej. "Programación", "Diseño")
    public ushort Rank { get; set; }
    public string IconUrl { get; set; }

    [ForeignKey(nameof(Skill))] public long SkillId { get; set; }

    public virtual SkillEntity Skill { get; set; }
}