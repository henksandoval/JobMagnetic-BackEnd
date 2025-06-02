using JobMagnet.Application.Exceptions;
using JobMagnet.Application.UseCases.CvParser.Commands;
using JobMagnet.Application.UseCases.CvParser.Mappers;
using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Application.UseCases.CvParser;

public interface ICvParserHandler
{
    Task ParseAsync(CvParserCommand command, CancellationToken cancellationToken = default);
}

public class CvParserHandler(
    IRawCvParser cvParser,
    IUnitOfWork unitOfWork,
    IQueryRepository<ContactTypeEntity, long> contactTypeQueryRepository)
    : ICvParserHandler
{
    private readonly IRawCvParser _cvParser = cvParser ?? throw new ArgumentNullException(nameof(cvParser));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IQueryRepository<ContactTypeEntity, long> _contactTypeQueryRepository = contactTypeQueryRepository ?? throw new ArgumentNullException(nameof(contactTypeQueryRepository));

    public async Task ParseAsync(CvParserCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        var profileEntity = await ParseCvToProfileEntity(command);
        await PersistProfileAsync(profileEntity, cancellationToken);
    }

    private async Task<ProfileEntity> ParseCvToProfileEntity(CvParserCommand command)
    {
        var rawProfile = await _cvParser.ParseAsync(command.Stream);

        if (rawProfile.HasNoValue)
        {
            throw new JobMagnetApplicationException("Failed to parse the CV. The raw profile is empty.");
        }

        var parsedProfileDto = rawProfile.Value.ToProfileParseDto();
        var profileEntity = parsedProfileDto.ToProfileEntity();
        return profileEntity;
    }

    private async Task PersistProfileAsync(ProfileEntity profileEntity, CancellationToken cancellationToken)
    {
        await _unitOfWork.ExecuteOperationInTransactionAsync(async () =>
        {
            if (profileEntity.Resume?.ContactInfo is { Count: > 0 })
            {
                await ResolveContactTypesAsync(profileEntity.Resume.ContactInfo, cancellationToken).ConfigureAwait(false);
            }

            await _unitOfWork.ProfileRepository.CreateAsync(profileEntity, cancellationToken).ConfigureAwait(false);
        }, cancellationToken);
    }

    private void SetAuditingFields(ProfileEntity profile)
    {
        //TODO: Implement auditing fields setting logic
    }

    private async Task ResolveContactTypesAsync(
        ICollection<ContactInfoEntity>? contactInfo,
        CancellationToken cancellationToken = default)
    {
        if (contactInfo is null || contactInfo.Count == 0)
        {
            return;
        }

        var contactTypesFromDto = contactInfo
            .Select(inf => inf.ContactType.Name)
            .Where(name => !string.IsNullOrEmpty(name))
            .Distinct()
            .ToList();

        var existingContactTypes = await _contactTypeQueryRepository
            .FindAsync(type => contactTypesFromDto.Contains(type.Name), cancellationToken)
            .ConfigureAwait(false);

        var existingContactTypeNamesDictionary = existingContactTypes.ToDictionary(
            contactType => contactType.Name,
            contactType => contactType,
            StringComparer.OrdinalIgnoreCase);

        foreach (var info in contactInfo)
        {
            var typeName = info.ContactType.Name;

            if (existingContactTypeNamesDictionary.TryGetValue(typeName, out var existingContactType))
            {
                info.ContactType = existingContactType;
                info.ContactTypeId = existingContactType.Id;
                continue;
            }

            info.ContactType = new ContactTypeEntity
            {
                Id = 0,
                Name = typeName
            };
        }
    }
}