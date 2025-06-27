namespace JobMagnet.Domain.Shared.Base.Interfaces;

public interface IStronglyTypedId<out TValue> where TValue : notnull;

public interface IStronglyTypedId<out TSelf, out TValue>
    where TSelf : IStronglyTypedId<TSelf, TValue>
    where TValue : notnull
{
    TValue Value { get; }

    static abstract TSelf New();
}