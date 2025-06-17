using Mapster;
using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Application.UseCases.CvParser.Mappers;

public static class ProfileParseDtoMapper
{
    static ProfileParseDtoMapper()
    {
        RegisterMaps();
    }

    public static ProfileEntity ToProfileEntity(this ProfileParseDto profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        return profile.Adapt<ProfileEntity>();
    }

    private static void RegisterMaps()
    {
        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

        TypeAdapterConfig<ProfileParseDto, ProfileEntity>.NewConfig()
            .Map(dest => dest.SkillSet, src => MapSkillSet(src.SkillSet!))
            .Map(dest => dest.Talents, src => MapTalents(src))
            .Map(dest => dest.Testimonials, src => MapTestimonials(src))
            .Map(dest => dest.PortfolioGallery, src => MapPortfolio(src));

        TypeAdapterConfig<ResumeParseDto, ResumeEntity>
            .NewConfig()
            .Map(dest => dest.About, src => src.About ?? string.Empty)
            .Map(dest => dest.Overview, src => src.Overview ?? string.Empty)
            .Map(dest => dest.Summary, src => src.Summary ?? string.Empty)
            .Map(dest => dest.ContactInfo, src => MapContactInfo(src));

        TypeAdapterConfig<ServiceParseDto, ServiceEntity>
            .NewConfig()
            .Map(dest => dest.Overview, src => src.Overview ?? string.Empty);

        TypeAdapterConfig<GalleryItemParseDto, ServiceGalleryItemEntity>.NewConfig();

        TypeAdapterConfig<SummaryParseDto, SummaryEntity>
            .NewConfig()
            .Map(dest => dest.Introduction, src => src.Introduction ?? string.Empty);

        TypeAdapterConfig<EducationParseDto, EducationEntity>.NewConfig()
            .Map(dest => dest.StartDate, src => ToDateTimeOrDefault(src.StartDate))
            .Map(dest => dest.EndDate, src => ToNullableDateTime(src.EndDate))
            .Map(dest => dest.Description, src => src.Description ?? string.Empty)
            .Map(dest => dest.InstitutionName, src => src.InstitutionName ?? string.Empty)
            .Map(dest => dest.InstitutionLocation, src => src.InstitutionLocation ?? string.Empty);

        TypeAdapterConfig<WorkExperienceParseDto, WorkExperienceEntity>.NewConfig()
            .Map(dest => dest.StartDate, src => ToDateTimeOrDefault(src.StartDate))
            .Map(dest => dest.EndDate, src => ToNullableDateTime(src.EndDate))
            .Map(dest => dest.Description, src => src.Description ?? string.Empty)
            .Map(dest => dest.CompanyName, src => src.CompanyName ?? string.Empty)
            .Map(dest => dest.CompanyLocation, src => src.CompanyLocation ?? string.Empty);

        TypeAdapterConfig<TalentParseDto, TalentEntity>.NewConfig();

        TypeAdapterConfig<PortfolioGalleryParseDto, PortfolioGalleryEntity>.NewConfig();

        TypeAdapterConfig<TestimonialParseDto, TestimonialEntity>.NewConfig();
    }

    private static DateTime ToDateTimeOrDefault(DateOnly? date)
    {
        return date?.ToDateTime(TimeOnly.MinValue) ?? default;
    }

    private static DateTime? ToNullableDateTime(DateOnly? date)
    {
        return date?.ToDateTime(TimeOnly.MinValue) ?? null;
    }

    private static List<TalentEntity> MapTalents(ProfileParseDto src)
    {
        return src is not { Talents: null } ? src.Talents.Adapt<List<TalentEntity>>() : [];
    }

    private static List<PortfolioGalleryEntity> MapPortfolio(ProfileParseDto src)
    {
        return src.PortfolioGallery?.Count > 0 ? src.PortfolioGallery.Adapt<List<PortfolioGalleryEntity>>() : [];
    }

    private static List<TestimonialEntity> MapTestimonials(ProfileParseDto src)
    {
        return src is not { Testimonials: null } ? src.Testimonials.Adapt<List<TestimonialEntity>>() : [];
    }

    private static List<ContactInfoEntity> MapContactInfo(ResumeParseDto src)
    {
        if (src is { ContactInfo: null })
        {
            return [];
        }

        var result = src.ContactInfo.Select(c => new ContactInfoEntity
        {
            Id = 0,
            Value = c.Value!,
            ContactTypeId = 0,
            ContactType = new ContactTypeEntity(c.ContactType)
        }).ToList();

        return result;
    }

    private static SkillSetEntity MapSkillSet(SkillSetParseDto src)
    {
        var skillSet = new SkillSetEntity(src.Overview!, 0);

        var skills = src.Skills
            .Select((skillDto, i) =>
            {
                var oneBasedIndex = (ushort)(i + 1);

                return new SkillEntity(
                    Convert.ToUInt16(skillDto.Level),
                    oneBasedIndex,
                    skillSet,
                    new SkillType(0, skillDto.Name!, new SkillCategory(""))
                );
            })
            .ToList();

        skillSet.AddRange(skills);

        return skillSet;
    }
}