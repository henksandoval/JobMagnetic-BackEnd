namespace JobMagnet.Application.Exceptions;

internal class JobMagnetApplicationException(string message, Exception? innerException = null) : Exception(message, innerException);