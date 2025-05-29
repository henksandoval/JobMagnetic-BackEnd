namespace JobMagnet.Application.Models.Base;

public interface IIdentifierBase<T>
{
    T Id { get; init; }
}