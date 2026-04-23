namespace JobMagnet.Domain.Exceptions;

public class InvalidCredentialsException(string message) : Exception(message);