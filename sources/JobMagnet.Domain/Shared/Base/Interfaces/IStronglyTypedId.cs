namespace JobMagnet.Domain.Shared.Base.Interfaces;

public interface IStronglyTypedId<out TValue> where TValue : notnull;