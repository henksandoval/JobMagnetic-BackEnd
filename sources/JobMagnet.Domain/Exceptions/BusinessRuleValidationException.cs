namespace JobMagnet.Domain.Exceptions;

public class BusinessRuleValidationException(string message) : JobMagnetDomainException(message);