using JobMagnet.Domain.Shared.Base.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JobMagnet.Infrastructure.Persistence.Context.ValueConverters;

public class StronglyTypedIdValueConverter<TStronglyTypedId, TValue>() :
    ValueConverter<TStronglyTypedId, TValue>(
        id => (TValue)id.GetType().GetProperty("Value")!.GetValue(id)!,
        value => (TStronglyTypedId)Activator.CreateInstance(typeof(TStronglyTypedId), value)!
    ) where TStronglyTypedId : IStronglyTypedId<TStronglyTypedId>;