namespace JobMagnet.Domain.Shared.Base;

public interface IStronglyTypedId<out TValue> where TValue : notnull
{
    TValue Value { get; }
}