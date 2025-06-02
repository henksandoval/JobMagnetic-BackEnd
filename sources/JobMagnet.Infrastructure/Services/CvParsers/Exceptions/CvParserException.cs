namespace JobMagnet.Infrastructure.ExternalServices.CvParsers.Exceptions;

internal class CvParserException(string message, Exception innerException) : Exception(message, innerException);