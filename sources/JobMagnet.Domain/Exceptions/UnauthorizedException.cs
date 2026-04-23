namespace JobMagnet.Domain.Exceptions;

public class UnauthorizedException(string message, Exception? innerException = null)
    : Exception(message, innerException);