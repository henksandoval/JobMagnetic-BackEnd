using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class SkillItemEntity : TrackableEntity<long>
{
    public string Name { get; set; } // Nombre de la habilidad (ej. "C#", "React")
    public ushort ProficiencyLevel { get; set; } // Nivel de habilidad (1-10)
    public string Category { get; set; } // Categoría (ej. "Programación", "Diseño")
    public ushort Rank { get; set; }
    public string IconUrl { get; set; }  //TODO: Refactor this property, remove it and use a separate entity for icons

    [ForeignKey(nameof(Skill))] public long SkillId { get; set; }

    public virtual SkillEntity Skill { get; set; }

    private SkillItemEntity() { }

    [SetsRequiredMembers]
    public SkillItemEntity(string name, string iconUrl, string category, SkillEntity skill, ushort proficiencyLevel, ushort rank, long id = 0)
    {
        Guard.IsNotNullOrWhiteSpace(name);
        Guard.IsNotNullOrWhiteSpace(iconUrl);
        Guard.IsNotNullOrWhiteSpace(category);
        Guard.IsNotNull(skill);
        Guard.IsGreaterThan<ushort>(proficiencyLevel, 0);
        Guard.IsGreaterThan<ushort>(rank, 0);
        Guard.IsGreaterThanOrEqualTo<long>(id, 0);

        Id = id;
        Name = name;
        IconUrl = iconUrl;
        Category = category;
        ProficiencyLevel = proficiencyLevel;
        Rank = 0;
        SkillId = skill.Id;
        Skill = skill;
    }
}