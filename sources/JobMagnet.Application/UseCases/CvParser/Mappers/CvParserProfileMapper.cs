using JobMagnet.Application.UseCases.CvParser.RawDTOs;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

namespace JobMagnet.Application.UseCases.CvParser.Mappers
{
    public interface ICvParserProfileMapper
    {
        ProfileParseDto MapFromRaw(ProfileRaw? rawDto);
    }

    public class CvParserProfileMapper : ICvParserProfileMapper
    {
        public ProfileParseDto MapFromRaw(ProfileRaw? rawDto)
        {
            if (rawDto == null)
            {
                // Devolver un ProfileParseDto vacío con listas inicializadas
                return new ProfileParseDto
                {
                    Talents = new List<TalentParseDto>(),
                    PortfolioGallery = new List<PortfolioGalleryParseDto>(),
                    Testimonials = new List<TestimonialParseDto>()
                };
            }

            var parsedDto = new ProfileParseDto
            {
                FirstName = rawDto.FirstName,
                LastName = rawDto.LastName,
                MiddleName = rawDto.MiddleName,
                SecondLastName = rawDto.SecondLastName,
                ProfileImageUrl = rawDto.ProfileImageUrl,
                BirthDate = FlexibleDateParserUtil.ParseFlexibleDate(rawDto.BirthDate),

                Resume = rawDto.Resume != null ? MapResume(rawDto.Resume) : null,
                Skill = rawDto.Skill != null ? MapSkill(rawDto.Skill) : null,
                Services = rawDto.Services != null ? MapService(rawDto.Services) : null,
                Summary = rawDto.Summary != null ? MapSummary(rawDto.Summary) : null,

                Talents = rawDto.Talents?.Select(MapTalent).ToList() ?? new List<TalentParseDto>(),
                PortfolioGallery = rawDto.PortfolioGallery?.Select(MapPortfolioGalleryItem).ToList() ?? new List<PortfolioGalleryParseDto>(),
                Testimonials = rawDto.Testimonials?.Select(MapTestimonial).ToList() ?? new List<TestimonialParseDto>()
            };

            return parsedDto;
        }

        private ResumeParseDto MapResume(ResumeRaw raw) => new()
        {
            JobTitle = raw.JobTitle,
            About = raw.About,
            Summary = raw.Summary,
            Overview = raw.Overview,
            Title = raw.Title,
            Suffix = raw.Suffix,
            Address = raw.Address,
            ContactInfo = raw.ContactInfo?.Select(ci => new ContactInfoParseDto { ContactType = ci.ContactType, Value = ci.Value }).ToList() ?? new List<ContactInfoParseDto>()
        };

        private SkillParseDto MapSkill(SkillRaw raw) => new()
        {
            Overview = raw.Overview,
            SkillDetails = raw.SkillDetails?.Select(sd => new SkillDetailParseDto { Name = sd.Name, Level = Convert.ToUInt16(sd.Level) }).ToList() ?? new List<SkillDetailParseDto>()
        };

        private ServiceParseDto MapService(ServiceRaw raw) => new()
        {
            Overview = raw.Overview,
            GalleryItems = raw.GalleryItems?.Select(gi => new GalleryItemParseDto { Title = gi.Title, Description = gi.Description, UrlLink = gi.UrlLink }).ToList() ?? new List<GalleryItemParseDto>()
        };

        private SummaryParseDto MapSummary(SummaryRaw raw) => new()
        {
            Introduction = raw.Introduction,
            Education = raw.Education?.Select(MapEducation).ToList() ?? new List<EducationParseDto>(),
            WorkExperiences = raw.WorkExperiences?.Select(MapWorkExperience).ToList() ?? new List<WorkExperienceParseDto>()
        };

        private EducationParseDto MapEducation(EducationRaw raw) => new()
        {
            InstitutionName = raw.InstitutionName,
            InstitutionLocation = raw.InstitutionLocation,
            Degree = raw.Degree,
            StartDate = FlexibleDateParserUtil.ParseFlexibleDate(raw.StartDate),
            EndDate = FlexibleDateParserUtil.ParseFlexibleDate(raw.EndDate)
        };

        private WorkExperienceParseDto MapWorkExperience(WorkExperienceRaw raw) => new()
        {
            CompanyName = raw.CompanyName,
            CompanyLocation = raw.CompanyLocation,
            StartDate = FlexibleDateParserUtil.ParseFlexibleDate(raw.StartDate),
            EndDate = FlexibleDateParserUtil.ParseFlexibleDate(raw.EndDate),
            Description = raw.Description
        };

        private TalentParseDto MapTalent(TalentRaw raw) => new() { Description = raw.Description };

        private PortfolioGalleryParseDto MapPortfolioGalleryItem(PortfolioGalleryRaw raw) => new()
        {
            Position = raw.Position,
            Title = raw.Title,
            Description = raw.Description,
            UrlLink = raw.UrlLink,
            UrlImage = raw.UrlImage,
            UrlVideo = raw.UrlVideo,
            Type = raw.Type
        };

        private TestimonialParseDto MapTestimonial(TestimonialRaw raw) => new()
        {
            Name = raw.Name,
            JobTitle = raw.JobTitle,
            PhotoUrl = raw.PhotoUrl,
            Feedback = raw.Feedback
        };
    }
}