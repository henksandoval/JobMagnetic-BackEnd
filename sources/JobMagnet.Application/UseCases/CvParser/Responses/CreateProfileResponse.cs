namespace JobMagnet.Application.UseCases.CvParser.Responses;

public sealed record CreateProfileResponse(long ProfileId, string UserEmail, string ProfileUrl);