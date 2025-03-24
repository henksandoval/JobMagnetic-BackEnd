namespace JobMagnet.Models.Shared;

public interface IIdentifierBase<T>
{
    T Id { get; init; }
}