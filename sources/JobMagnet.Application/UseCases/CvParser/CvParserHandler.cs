using JobMagnet.Application.UseCases.CvParser.Commands;
using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Application.UseCases.CvParser;

public interface ICvParserHandler
{
    Task ParseAsync(CvParserCommand command);
}

public class CvParserHandler(IRawCvParser cvParser, ICommandRepository<ProfileEntity> commandRepository) : ICvParserHandler
{
    public async Task ParseAsync(CvParserCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var response = await cvParser.ParseAsync(command.Stream);

        if (response.HasValue)
        {
        }
    }
}