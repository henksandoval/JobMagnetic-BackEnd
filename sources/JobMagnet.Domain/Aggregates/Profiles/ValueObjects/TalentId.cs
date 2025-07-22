using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public readonly record struct TalentId(Guid Value) : IStronglyTypedId<TalentId>;