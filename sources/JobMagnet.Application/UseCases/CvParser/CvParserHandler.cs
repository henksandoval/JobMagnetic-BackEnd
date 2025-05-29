using JobMagnet.Application.Mappers;
using JobMagnet.Application.UseCases.CvParser.Commands;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using JobMagnet.Domain.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Domain.Services.CvParser;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Application.UseCases.CvParser;

public interface ICvParserHandler
{
    Task ParseAsync(CvParserCommand command);
}

public class CvParserHandler(ICvParser cvParser, ICommandRepository<ProfileEntity> commandRepository) : ICvParserHandler
{
    public async Task ParseAsync(CvParserCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var response = await cvParser.ParseAsync(command.Stream);

        if (response.HasValue)
        {
            var parsedProfile = response.Value as ProfileParseDto;
            var profileCommand = parsedProfile!.ToCommand();
        }
    }
}