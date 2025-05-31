using CSharpFunctionalExtensions;
using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Domain.Domain.Services.CvParser;

public interface ICvParser
{
    Task<Maybe<IParsedProfile>> ParseAsync(Stream cvFile);
}