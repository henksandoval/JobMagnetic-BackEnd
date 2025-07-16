using System.Diagnostics.CodeAnalysis;

namespace JobMagnet.Infrastructure.Services.CvParsers.Exceptions;

[SuppressMessage(
    "SonarQube",
    "S3874",
    Justification = "False positive - inherits from JobMagnetDomainException which inherits from Exception")]
internal class CvParserException(string message, Exception innerException) : Exception(message, innerException);