namespace JobMagnet.Infrastructure.DTO.Profile;

// ReSharper disable once ClassNeverInstantiated.Global
public class SkillItemDTO
{
    public string Name { get; set; } // Nombre de la habilidad (ej. "C#", "React")
    public ushort ProficiencyLevel { get; set; } // Nivel de habilidad (1-10)
    public string Category { get; set; } // Categoría (ej. "Programación", "Diseño")
    public ushort Rank { get; set; }
    public string IconUrl { get; set; }

    public virtual SkillDTO Skill { get; set; }
}