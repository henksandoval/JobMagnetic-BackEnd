using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Aggregates;

public readonly record struct UserId(Guid Value) : IStronglyTypedId<UserId>;