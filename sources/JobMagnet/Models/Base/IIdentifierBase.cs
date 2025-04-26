namespace JobMagnet.Models.Base;

public interface IIdentifierBase<T>
{
    T Id { get; init; }
}