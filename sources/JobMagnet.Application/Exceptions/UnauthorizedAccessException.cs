namespace JobMagnet.Application.Exceptions;

public class UnauthorizedException(string message, Exception? innerException = null) 
    : Exception(message, innerException);