using CSharpFunctionalExtensions;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Application.UseCases.CvParser.Ports;

public interface IRawCvParser
{
    Task<Maybe<ProfileRaw>> ParseAsync(Stream cvFile);
}