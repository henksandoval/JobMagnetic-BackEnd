using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public readonly record struct SkillSetId(Guid Value) : IStronglyTypedId<SkillSetId>;