using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public readonly record struct QualificationId(Guid Value) : IStronglyTypedId<QualificationId>;