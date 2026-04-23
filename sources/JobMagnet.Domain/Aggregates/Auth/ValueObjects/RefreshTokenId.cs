using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Aggregates.Auth.ValueObjects;

public readonly record struct RefreshTokenId(Guid Value) : IStronglyTypedId<RefreshTokenId>;