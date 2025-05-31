namespace JobMagnet.Domain.Core.Services.CvParser.Interfaces;

public interface IParsedTestimonial
{
    string? Name { get; }
    string? JobTitle { get; }
    string? PhotoUrl { get; }
    string? Feedback { get; }
}