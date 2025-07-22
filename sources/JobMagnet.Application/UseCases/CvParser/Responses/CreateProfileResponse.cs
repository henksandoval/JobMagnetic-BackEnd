namespace JobMagnet.Application.UseCases.CvParser.Responses;

public sealed record CreateProfileResponse(Guid ProfileId, string UserEmail, string ProfileUrl);