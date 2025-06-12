using CSharpFunctionalExtensions;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Services;

public interface IContactTypeResolverService
{
    Task<Maybe<ContactTypeEntity>> ResolveAsync(string nameOrAlias, CancellationToken cancellationToken);
}

public class ContactTypeResolverService(
    IQueryRepository<ContactTypeEntity, int> contactTypeRepository,
    IQueryRepository<ContactTypeAliasEntity, int> contactTypeAliasRepository)
    : IContactTypeResolverService
{
    public async Task<Maybe<ContactTypeEntity>> ResolveAsync(string nameOrAlias, CancellationToken cancellationToken)
    {
        var contactType = await contactTypeRepository.FirstOrDefaultAsync(
            ct => ct.Name.Equals(nameOrAlias, StringComparison.OrdinalIgnoreCase),
            cancellationToken);

        if (contactType is not null)
        {
            return Maybe.From(contactType);
        }

        var alias = await contactTypeAliasRepository.FirstOrDefaultAsync(
            a => a.Alias.Equals(nameOrAlias, StringComparison.OrdinalIgnoreCase),
            cancellationToken);

        if (alias is null || !alias.ContactTypeExist)
        {
            return Maybe.None;
        }

        var finalContactType = await contactTypeRepository.GetByIdAsync(alias.ContactTypeId, cancellationToken);

        return Maybe.From(finalContactType);
    }
}