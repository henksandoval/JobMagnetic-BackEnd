namespace JobMagnet.Infrastructure.Services.CvParsers.Exceptions;

internal class CvParserException(string message, Exception innerException) : Exception(message, innerException);