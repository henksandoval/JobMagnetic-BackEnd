using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;
using Newtonsoft.Json;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class ProfileParseDto : IParsedProfile
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfileImageUrl { get; set; }

    [JsonConverter(typeof(FlexibleDateOnlyConverter))]
    public DateOnly? BirthDate { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }

    public ResumeParseDto? Resume { private get; set; }
    public SkillParseDto? Skill { private get; set; }
    public ServiceParseDto? Services { private get; set; }
    public SummaryParseDto? Summary { private get; set; }
    public IEnumerable<TalentParseDto> TalentList { private get; set; }
    public IEnumerable<PortfolioGalleryParseDto> PortfolioGalleryList { private get; set; }
    public IEnumerable<TestimonialParseDto> TestimonialList { private get; set; }

    IParsedResume? IParsedProfile.Resume => Resume;
    IParsedSkill? IParsedProfile.Skill => Skill;
    IParsedService? IParsedProfile.Services => Services;
    IParsedSummary? IParsedProfile.Summary => Summary;

    public IReadOnlyCollection<IParsedTalent> Talents =>
        new List<IParsedTalent>(TalentList).AsReadOnly();

    public IReadOnlyCollection<IParsedPortfolioGallery> PortfolioGallery =>
        new List<IParsedPortfolioGallery>(PortfolioGalleryList).AsReadOnly();

    public IReadOnlyCollection<IParsedTestimonial> Testimonials =>
        new List<IParsedTestimonial>(TestimonialList).AsReadOnly();
}