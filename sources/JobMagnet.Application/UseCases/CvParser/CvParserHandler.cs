using JobMagnet.Application.Exceptions;
using JobMagnet.Application.Factories;
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
    ICommandRepository<Profile> profileRepository,
    IProfileSlugGenerator slugGenerator,
    IProfileFactory profileFactory)
    : ICvParserHandler
{
    public async Task<CreateProfileResponse> ParseAsync(CvParserCommand command, CancellationToken cancellationToken = default)
    {
        var profileEntity = await BuildProfileFromCvAsync(command, cancellationToken);

        profileEntity.LinkManager.CreateAndAssignPublicIdentifier(slugGenerator);

        await profileRepository.CreateAsync(profileEntity, cancellationToken);
        await profileRepository.SaveChangesAsync(cancellationToken);

        var userEmail = GetUserEmail(profileEntity);
        var profileUrl = GetProfileSlugUrl(profileEntity);

        return new CreateProfileResponse(profileEntity.Id, userEmail, profileUrl);
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
        var userEmail = profile.Resume?.ContactInfo?
                            .FirstOrDefault(x => x.ContactType.Name.Equals("Email", StringComparison.OrdinalIgnoreCase))?
                            .Value
                        ?? string.Empty;
        return userEmail;
    }

    private static string GetProfileSlugUrl(Profile profile)
    {
        return profile.VanityUrls!.SingleOrDefault(x => x.Type == LinkType.Primary)!.ProfileSlugUrl;
    }
}