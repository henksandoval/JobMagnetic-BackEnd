using JobMagnet.Application.Exceptions;
using JobMagnet.Application.Factories;
using JobMagnet.Application.Services;
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
    ICommandRepository<ProfileEntity> profileRepository,
    IProfileSlugGenerator slugGenerator,
    IProfileFactory profileFactory,
    IContactTypeResolverService contactTypeResolver,
    ISkillTypeResolverService skillTypeResolver)
    : ICvParserHandler
{
    public async Task<CreateProfileResponse> ParseAsync(CvParserCommand command, CancellationToken cancellationToken = default)
    {
        var profileEntity = await BuildProfileFromCvAsync(command, cancellationToken);

        profileEntity.CreateAndAssignPublicIdentifier(slugGenerator);

        await profileRepository.CreateAsync(profileEntity, cancellationToken);
        await profileRepository.SaveChangesAsync(cancellationToken);

        var userEmail = GetUserEmail(profileEntity);
        var profileUrl = GetProfileSlugUrl(profileEntity);

        return new CreateProfileResponse(profileEntity.Id, userEmail, profileUrl);
    }

    private async Task<ProfileEntity> BuildProfileFromCvAsync(CvParserCommand command, CancellationToken cancellationToken)
    {
        var rawProfile = await cvParser.ParseAsync(command.Stream);
        if (rawProfile.HasNoValue)
        {
            throw new JobMagnetApplicationException("Failed to parse the CV.");
        }

        var profileParse = rawProfile.Value.ToProfileParseDto();
        var profileEntity = await profileFactory.CreateProfileFromDtoAsync(profileParse, cancellationToken);

        if (profileEntity.Resume?.ContactInfo is { Count: > 0 })
        {
            await ResolveAndAssignContactTypesAsync(profileEntity.Resume.ContactInfo, cancellationToken);
        }

        if (profileEntity.SkillSet?.Skills is { Count: > 0 })
        {
            await ResolveAndAssignSkillsAsync(profileEntity.SkillSet.Skills, cancellationToken);
        }

        return profileEntity;
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

    private async Task ResolveAndAssignSkillsAsync(ICollection<SkillEntity>? skills, CancellationToken cancellationToken)
    {
        if (skills is null || skills.Count == 0)
        {
            return;
        }

        foreach (var skill in skills)
        {
            var rawSkill = skill.SkillType.Name;
            if (string.IsNullOrWhiteSpace(rawSkill)) continue;

            var resolvedType = await skillTypeResolver.ResolveAsync(rawSkill, cancellationToken);

            if (resolvedType.HasValue)
            {
                continue;
            }

        }
    }
}