using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using Mapster;

namespace JobMagnet.Application.UseCases.CvParser.Mappers;

public static class CvParserMapper
{
    static CvParserMapper()
    {
        RegisterMaps();
    }

    public static ProfileParseDto ToProfileParseDto(this ProfileRaw profileRaw)
    {
        return profileRaw.Adapt<ProfileParseDto>();
    }

    private static void RegisterMaps()
    {
        TypeAdapterConfig<ProfileRaw, ProfileParseDto>
            .NewConfig()
            .Map(dest => dest.BirthDate, src => FlexibleDateParserUtil.ParseFlexibleDate(src.BirthDate))
            .Map(dest => dest.Talents, src => MapTalents(src.Talents))
            .Map(dest => dest.PortfolioGallery, src => MapPortfolioGallery(src.PortfolioGallery))
            .Map(dest => dest.Testimonials, src => MapTestimonial(src.Testimonials));

        TypeAdapterConfig<ResumeRaw, ResumeParseDto>
            .NewConfig()
            .Map(dest => dest.ContactInfo, src => MapContactInfo(src.ContactInfo));

        TypeAdapterConfig<SkillRaw, SkillParseDto>
            .NewConfig()
            .Map(dest => dest.SkillDetails, src => MapSkills(src.SkillDetails));

        TypeAdapterConfig<SkillDetailRaw, SkillDetailParseDto>
            .NewConfig()
            .Map(dest => dest.Level, src => Convert.ToUInt16(src.Level));

        TypeAdapterConfig<ServiceRaw, ServiceParseDto>
            .NewConfig()
            .Map(dest => dest.GalleryItems, src => MapServices(src.GalleryItems));

        TypeAdapterConfig<SummaryRaw, SummaryParseDto>
            .NewConfig()
            .Map(dest => dest.Education, src => MapEducation(src.Education))
            .Map(dest => dest.WorkExperiences, src => MapWorkExperience(src.WorkExperiences));

        TypeAdapterConfig<EducationRaw, EducationParseDto>
            .NewConfig()
            .Map(dest => dest.StartDate, src => FlexibleDateParserUtil.ParseFlexibleDate(src.StartDate))
            .Map(dest => dest.EndDate, src => FlexibleDateParserUtil.ParseFlexibleDate(src.EndDate));

        TypeAdapterConfig<WorkExperienceRaw, WorkExperienceParseDto>
            .NewConfig()
            .Map(dest => dest.StartDate, src => FlexibleDateParserUtil.ParseFlexibleDate(src.StartDate))
            .Map(dest => dest.EndDate, src => FlexibleDateParserUtil.ParseFlexibleDate(src.EndDate));
    }

    private static List<WorkExperienceParseDto> MapWorkExperience(IEnumerable<WorkExperienceRaw>? srcWorkExperiences) =>
        srcWorkExperiences == null ? [] : srcWorkExperiences.Adapt<List<WorkExperienceParseDto>>();

    private static List<EducationParseDto> MapEducation(IEnumerable<EducationRaw>? srcEducation) =>
        srcEducation == null ? [] : srcEducation.Adapt<List<EducationParseDto>>();

    private static List<GalleryItemParseDto> MapServices(ICollection<GalleryItemRaw>? srcGallery) =>
        srcGallery == null ? [] : srcGallery.Adapt<List<GalleryItemParseDto>>();

    private static List<SkillDetailParseDto> MapSkills(IEnumerable<SkillDetailRaw>? srcSkills) =>
        srcSkills == null ? [] : srcSkills.Adapt<List<SkillDetailParseDto>>();

    private static List<TestimonialParseDto> MapTestimonial(List<TestimonialRaw>? srcTestimonials) =>
        srcTestimonials == null ? [] : srcTestimonials.Adapt<List<TestimonialParseDto>>();

    private static List<PortfolioGalleryParseDto> MapPortfolioGallery(List<PortfolioGalleryRaw>? srcPortfolio) =>
        srcPortfolio == null ? [] : srcPortfolio.Adapt<List<PortfolioGalleryParseDto>>();

    private static List<TalentParseDto> MapTalents(IEnumerable<TalentRaw>? srcTalents) =>
        srcTalents == null ? [] : srcTalents.Adapt<List<TalentParseDto>>();

    private static List<ContactInfoParseDto> MapContactInfo(IEnumerable<ContactInfoRaw>? srcContactInfo) =>
        srcContactInfo == null ? [] : srcContactInfo.Adapt<List<ContactInfoParseDto>>();
}