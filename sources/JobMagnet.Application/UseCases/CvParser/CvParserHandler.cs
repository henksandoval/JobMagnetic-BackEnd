using JobMagnet.Application.Exceptions;
using JobMagnet.Application.Factories;
using JobMagnet.Application.UseCases.CvParser.Commands;
using JobMagnet.Application.UseCases.CvParser.Mappers;
using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Application.UseCases.CvParser.Responses;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Services;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Application.UseCases.CvParser;

public interface ICvParserHandler
{
    Task<CreateProfileResponse> ParseAsync(CvParserCommand command, CancellationToken cancellationToken = default);
}

public class CvParserHandler(
    IGuidGenerator guidGenerator,
    IClock clock,
    IRawCvParser cvParser,
    IGenericCommandRepository<Profile> profileRepository,
    IProfileSlugGenerator slugGenerator,
    IProfileFactory profileFactory)
    : ICvParserHandler
{
    public async Task<CreateProfileResponse> ParseAsync(CvParserCommand command, CancellationToken cancellationToken = default)
    {
        var profileEntity = await BuildProfileFromCvAsync(command, cancellationToken);

        profileEntity.LinkManager.CreateAndAssignPublicIdentifier(guidGenerator, clock, slugGenerator);

        await profileRepository.CreateAsync(profileEntity, cancellationToken);
        await profileRepository.SaveChangesAsync(cancellationToken);

        var userEmail = GetUserEmail(profileEntity);
        var profileUrl = GetProfileSlugUrl(profileEntity);

        return new CreateProfileResponse(profileEntity.Id.Value, userEmail, profileUrl);
    }

    private async Task<Profile> BuildProfileFromCvAsync(CvParserCommand command, CancellationToken cancellationToken)
    {
        var rawProfile = await cvParser.ParseAsync(command.Stream);
        if (rawProfile.HasNoValue) throw new JobMagnetApplicationException("Failed to parse the CV.");

        var profileParse = rawProfile.Value.ToProfileParseDto();
        return await profileFactory.CreateProfileFromDtoAsync(profileParse, cancellationToken);
    }

    private static string GetUserEmail(Profile profile)
    {
        var userEmail = profile.ProfileHeader?.ContactInfo?
                            .FirstOrDefault(x => x.ContactType.Name.Equals("Email", StringComparison.OrdinalIgnoreCase))?
                            .Value
                        ?? string.Empty;
        return userEmail;
    }

    private static string GetProfileSlugUrl(Profile profile)
    {
        return profile.VanityUrls.SingleOrDefault(x => x.Type == LinkType.Primary)!.ProfileSlugUrl;
    }
}