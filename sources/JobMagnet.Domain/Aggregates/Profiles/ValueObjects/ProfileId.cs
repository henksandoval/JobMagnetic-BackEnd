using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public readonly record struct ProfileId(Guid Value) : IStronglyTypedId<ProfileId>;