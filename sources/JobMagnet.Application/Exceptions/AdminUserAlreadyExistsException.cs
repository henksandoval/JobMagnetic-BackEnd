namespace JobMagnet.Application.Exceptions;

public class AdminUserAlreadyExistsException : Exception
{
    public AdminUserAlreadyExistsException(string message) : base(message)
    {
    }

    public AdminUserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}