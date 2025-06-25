using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Shared.Utils;
using Mapster;

namespace JobMagnet.Application.UseCases.CvParser.Mappers;

public static class CvParserMapper
{
    static CvParserMapper()
    {
        RegisterMaps();
    }

    public static ProfileParseDto ToProfileParseDto(this ProfileRaw profileRaw) => profileRaw.Adapt<ProfileParseDto>();

    private static void RegisterMaps()
    {
        TypeAdapterConfig<ProfileRaw, ProfileParseDto>
            .NewConfig()
            .Map(dest => dest.BirthDate, src => src.BirthDate.ParseToDateOnly())
            .Map(dest => dest.Talents, src => MapTalents(src.Talents))
            .Map(dest => dest.Project, src => MapProject(src.Project))
            .Map(dest => dest.Testimonials, src => MapTestimonial(src.Testimonials));

        TypeAdapterConfig<ResumeRaw, ResumeParseDto>
            .NewConfig()
            .Map(dest => dest.ContactInfo, src => MapContactInfo(src.ContactInfo));

        TypeAdapterConfig<SkillSetRaw, SkillSetParseDto>
            .NewConfig()
            .Map(dest => dest.Skills, src => MapSkills(src.Skills));

        TypeAdapterConfig<SkillRaw, SkillParseDto>
            .NewConfig()
            .Map(dest => dest.Level, src => Convert.ToUInt16(src.Level));

        TypeAdapterConfig<SummaryRaw, SummaryParseDto>
            .NewConfig()
            .Map(dest => dest.Education, src => MapEducation(src.Education))
            .Map(dest => dest.WorkExperiences, src => MapWorkExperience(src.WorkExperiences));

        TypeAdapterConfig<EducationRaw, EducationParseDto>
            .NewConfig()
            .Map(dest => dest.StartDate, src => src.StartDate.ParseToDateOnly())
            .Map(dest => dest.EndDate, src => src.EndDate.ParseToDateOnly());

        TypeAdapterConfig<WorkExperienceRaw, WorkExperienceParseDto>
            .NewConfig()
            .Map(dest => dest.StartDate, src => src.StartDate.ParseToDateOnly())
            .Map(dest => dest.EndDate, src => src.EndDate.ParseToDateOnly());
    }

    private static List<WorkExperienceParseDto> MapWorkExperience(IEnumerable<WorkExperienceRaw>? srcWorkExperiences) =>
        srcWorkExperiences == null ? [] : srcWorkExperiences.Adapt<List<WorkExperienceParseDto>>();

    private static List<EducationParseDto> MapEducation(IEnumerable<EducationRaw>? srcEducation) =>
        srcEducation == null ? [] : srcEducation.Adapt<List<EducationParseDto>>();

    private static List<SkillParseDto> MapSkills(IEnumerable<SkillRaw>? srcSkills) =>
        srcSkills == null ? [] : srcSkills.Adapt<List<SkillParseDto>>();

    private static List<TestimonialParseDto> MapTestimonial(List<TestimonialRaw>? srcTestimonials) =>
        srcTestimonials == null ? [] : srcTestimonials.Adapt<List<TestimonialParseDto>>();

    private static List<ProjectParseDto> MapProject(List<ProjectRaw>? srcProject) =>
        srcProject == null ? [] : srcProject.Adapt<List<ProjectParseDto>>();

    private static List<TalentParseDto> MapTalents(IEnumerable<TalentRaw>? srcTalents) =>
        srcTalents == null ? [] : srcTalents.Adapt<List<TalentParseDto>>();

    private static List<ContactInfoParseDto> MapContactInfo(IEnumerable<ContactInfoRaw>? srcContactInfo) =>
        srcContactInfo == null ? [] : srcContactInfo.Adapt<List<ContactInfoParseDto>>();
}