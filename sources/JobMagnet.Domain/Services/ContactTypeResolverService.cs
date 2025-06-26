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
                    c.Name.Equals(nameOrAlias, StringComparison.CurrentCultureIgnoreCase) ||
                    c.Aliases.Any(a => a.Alias.Equals(nameOrAlias, StringComparison.CurrentCultureIgnoreCase)),
                cancellationToken);

        return Maybe.From(contactType);
    }
}