namespace JobMagnet.Domain.Exceptions;

internal class JobMagnetDomainException(string message, Exception? innerException = null) : Exception(message, innerException);