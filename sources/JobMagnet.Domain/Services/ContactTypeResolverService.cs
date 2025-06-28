using CSharpFunctionalExtensions;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Services;

public interface IContactTypeResolverService
{
    Task<Maybe<ContactType>> ResolveAsync(string nameOrAlias, CancellationToken cancellationToken);
}

public class ContactTypeResolverService(
    IQueryRepository<ContactType, int> contactTypeRepository)
    : IContactTypeResolverService
{
    public async Task<Maybe<ContactType>> ResolveAsync(string nameOrAlias, CancellationToken cancellationToken)
    {
        var contactType = await contactTypeRepository
            .FirstOrDefaultAsync(c =>
                    StringComparer.OrdinalIgnoreCase.Compare(c.Name, nameOrAlias) == 0 ||
                    c.Aliases.Any(a => StringComparer.OrdinalIgnoreCase.Compare(a.Alias, nameOrAlias) == 0),
                cancellationToken);

        return Maybe.From(contactType);
    }
}