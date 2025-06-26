using CSharpFunctionalExtensions;
using JobMagnet.Domain.Core.Entities.Contact;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Services;

public interface IContactTypeResolverService
{
    Task<Maybe<ContactType>> ResolveAsync(string nameOrAlias, CancellationToken cancellationToken);
}

public class ContactTypeResolverService(
    IQueryRepository<ContactType, int> contactTypeRepository,
    IQueryRepository<ContactTypeAlias, int> contactTypeAliasRepository)
    : IContactTypeResolverService
{
    public async Task<Maybe<ContactType>> ResolveAsync(string nameOrAlias, CancellationToken cancellationToken)
    {
        var contactType = await contactTypeRepository
            .FirstOrDefaultAsync(c =>
                    c.Name.ToLower() == nameOrAlias.ToLower(),
                cancellationToken);

        if (contactType is not null) return Maybe.From(contactType);

        var alias = await contactTypeAliasRepository.FirstOrDefaultAsync(
            a => a.Alias.ToLower() == nameOrAlias.ToLower(),
            cancellationToken);

        if (alias is null || !alias.ContactTypeExist) return Maybe.None;

        var finalContactType = await contactTypeRepository.GetByIdAsync(alias.ContactTypeId, cancellationToken);

        return Maybe.From(finalContactType);
    }
}