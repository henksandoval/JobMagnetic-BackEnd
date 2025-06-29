namespace JobMagnet.Domain.Exceptions;

public class JobMagnetDomainException(string message, Exception? innerException = null) : Exception(message, innerException);