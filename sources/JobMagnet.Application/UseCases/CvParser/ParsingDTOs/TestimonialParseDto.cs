using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class TestimonialParseDto : IParsedTestimonial
{
    public string? Name { get; set; }
    public string? JobTitle { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Feedback { get; set; }
}