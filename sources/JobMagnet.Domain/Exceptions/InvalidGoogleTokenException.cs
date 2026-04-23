namespace JobMagnet.Domain.Exceptions;

public class InvalidGoogleTokenException(string message)
    : Exception(message);