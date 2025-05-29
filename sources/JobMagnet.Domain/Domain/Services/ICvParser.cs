using CSharpFunctionalExtensions;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Domain.Domain.Services;

public interface ICvParser
{
    Task<Maybe<ProfileEntity>> ParseAsync(Stream cvFile);
}