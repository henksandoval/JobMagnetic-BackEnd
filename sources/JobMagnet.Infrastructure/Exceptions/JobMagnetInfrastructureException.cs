namespace JobMagnet.Infrastructure.Exceptions;

internal class JobMagnetInfrastructureException(string message, Exception? innerException = null) : Exception(message, innerException);