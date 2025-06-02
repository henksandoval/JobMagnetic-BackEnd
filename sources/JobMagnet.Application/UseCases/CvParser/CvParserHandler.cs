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

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var rawProfile = await _cvParser.ParseAsync(command.Stream);

            if (rawProfile.HasNoValue)
            {
                throw new JobMagnetApplicationException("Failed to parse the CV. The raw profile is empty.");
            }

            var parsedProfileDto = rawProfile.Value.ToProfileParseDto();
            var profileEntity = parsedProfileDto.ToProfileEntity();

            SetAuditingFields(profileEntity);

            if (profileEntity.Resume?.ContactInfo != null && profileEntity.Resume.ContactInfo.Count != 0)
            {
                await ResolveContactTypesAsync(profileEntity.Resume.ContactInfo, cancellationToken);
            }

            await _unitOfWork.ProfileRepository.CreateAsync(profileEntity, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private void SetAuditingFields(ProfileEntity profile)
    {
        //TODO: Implement auditing fields setting logic
    }

    private async Task ResolveContactTypesAsync(
        ICollection<ContactInfoEntity>? contactInfos,
        CancellationToken cancellationToken = default)
    {
        //TODO: Implement contact type resolution logic
        await Task.Delay(0, cancellationToken);
    }
}