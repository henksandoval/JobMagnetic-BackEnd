using CSharpFunctionalExtensions;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Domain.Profiles;

public interface ICvParser
{
    Task<Maybe<ProfileEntity>> ParseAsync(IFormFile file);
}