using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Aggregates.SkillTypes;

public readonly record struct SkillTypeId(Guid Value) : IStronglyTypedId<SkillTypeId>;