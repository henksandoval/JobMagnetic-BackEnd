using CSharpFunctionalExtensions;
using JobMagnet.Domain.Core.Services.CvParser.Interfaces;

namespace JobMagnet.Domain.Core.Services.CvParser;

public interface ICvParser
{
    Task<Maybe<IParsedProfile>> ParseAsync(Stream cvFile);
}