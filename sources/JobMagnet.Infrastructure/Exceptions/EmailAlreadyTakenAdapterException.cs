namespace JobMagnet.Infrastructure.Exceptions;

public class EmailAlreadyTakenAdapterException(string message) : Exception(message);