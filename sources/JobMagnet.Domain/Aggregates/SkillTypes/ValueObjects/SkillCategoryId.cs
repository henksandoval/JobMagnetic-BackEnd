using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Aggregates.SkillTypes.Entities;

public readonly record struct SkillCategoryId(Guid Value) : IStronglyTypedId<SkillCategoryId>;