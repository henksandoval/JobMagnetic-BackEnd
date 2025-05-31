namespace JobMagnet.Application.Contracts.Responses.Base;

public interface IIdentifierBase<T>
{
    T Id { get; init; }
}