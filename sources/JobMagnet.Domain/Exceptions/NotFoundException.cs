using JobMagnet.Domain.Shared.Base.Interfaces;

namespace JobMagnet.Domain.Exceptions;

public class NotFoundException(string message) : JobMagnetDomainException(message)
{
    public static NotFoundException For<TEntity, TValue>(IStronglyTypedId<TValue> id)
        where TEntity : class
        where TValue : notnull
    {
        var aggregateName = typeof(TEntity).Name;
        var message = $"The entity \"{aggregateName}\" with the ID \"{id}\" was not found.";
        return new NotFoundException(message);
    }
}