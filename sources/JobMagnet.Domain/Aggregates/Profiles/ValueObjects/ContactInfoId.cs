using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public readonly record struct ContactInfoId(Guid Value) : IStronglyTypedId<ContactInfoId>;