using JobMagnet.Application.Exceptions;
using JobMagnet.Application.UseCases.CvParser.Commands;
using JobMagnet.Application.UseCases.CvParser.Mappers;
using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Application.UseCases.CvParser.Responses;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Services;

namespace JobMagnet.Application.UseCases.CvParser;

public interface ICvParserHandler
{
    Task<CreateProfileResponse> ParseAsync(CvParserCommand command, CancellationToken cancellationToken = default);
}

public class CvParserHandler(
    IRawCvParser cvParser,
    IUnitOfWork unitOfWork,
    IProfileSlugGenerator slugGenerator,
    IContactTypeResolverService contactTypeResolver)
    : ICvParserHandler
{
    private readonly IRawCvParser _cvParser = cvParser ?? throw new ArgumentNullException(nameof(cvParser));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<CreateProfileResponse> ParseAsync(CvParserCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        var profileEntity = await ParseCvToProfileEntity(command);
        await PersistProfileAsync(profileEntity, cancellationToken);
        var userEmail = GetUserEmail(profileEntity);
        var profileUrl = GetProfileSlugUrl(profileEntity);
        return new CreateProfileResponse(profileEntity.Id, userEmail, profileUrl);
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
                await ResolveAndAssignContactTypesAsync(profileEntity.Resume.ContactInfo, cancellationToken).ConfigureAwait(false);
            }

            await _unitOfWork.ProfileRepository
                .CreateAsync(profileEntity, cancellationToken)
                .ConfigureAwait(false);

            var publicIdentifierEntity = new PublicProfileIdentifierEntity(profileEntity, slugGenerator);

            await _unitOfWork.PublicProfileIdentifierRepository
                .CreateAsync(publicIdentifierEntity, cancellationToken)
                .ConfigureAwait(false);

            profileEntity.AddPublicProfileIdentifier(publicIdentifierEntity);
        }, cancellationToken);
    }

    private static string GetUserEmail(ProfileEntity profileEntity)
    {
        var userEmail = profileEntity.Resume?.ContactInfo?
                            .FirstOrDefault(x => x.ContactType.Name.Equals("Email", StringComparison.OrdinalIgnoreCase))?
                            .Value
                        ?? string.Empty;
        return userEmail;
    }

    private static string GetProfileSlugUrl(ProfileEntity profileEntity)
    {
        return profileEntity.PublicProfileIdentifiers!.SingleOrDefault(x => x.Type == LinkType.Primary)!.ProfileSlugUrl;
    }

    private void SetAuditingFields(ProfileEntity profile)
    {
        //TODO: Implement auditing fields setting logic
    }

    private async Task ResolveAndAssignContactTypesAsync(
        ICollection<ContactInfoEntity>? contactInfoCollection,
        CancellationToken cancellationToken)
    {
        if (contactInfoCollection is null || contactInfoCollection.Count == 0)
        {
            return;
        }

        foreach (var info in contactInfoCollection)
        {
            var rawContactType = info.ContactType.Name;
            if (string.IsNullOrWhiteSpace(rawContactType)) continue;

            var resolvedType = await contactTypeResolver.ResolveAsync(rawContactType, cancellationToken);

            if (resolvedType.HasValue)
            {
                info.ContactType = resolvedType.Value;
                info.ContactTypeId = resolvedType.Value.Id;
                continue;
            }

            info.ContactTypeId = 0;
            info.ContactType = new ContactTypeEntity(rawContactType);
            info.ContactType.SetDefaultIcon();
        }
    }
}