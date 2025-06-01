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
        TypeAdapterConfig<ProfileParseDto, ProfileEntity>.NewConfig()
            .Map(dest => dest.Talents, src => MapTalents(src))
            .Map(dest => dest.Testimonials, src => MapTestimonials(src))
            .Map(dest => dest.PortfolioGallery, src => MapPortfolio(src));

        TypeAdapterConfig<ResumeParseDto, ResumeEntity>.NewConfig();

        TypeAdapterConfig<ContactInfoParseDto, ContactInfoEntity>.NewConfig()
            .Map(dest => dest.ContactType, src => new ContactTypeEntity
            {
                Id = 0,
                Name = src.ContactType ?? string.Empty
            });

        TypeAdapterConfig<SkillParseDto, SkillEntity>.NewConfig();

        TypeAdapterConfig<SkillDetailParseDto, SkillItemEntity>.NewConfig()
            .Map(dest => dest.ProficiencyLevel, src => src.Level);

        TypeAdapterConfig<ServiceParseDto, ServiceEntity>.NewConfig()
            .Map(dest => dest.Overview, src => src.Overview);

        TypeAdapterConfig<GalleryItemParseDto, ServiceGalleryItemEntity>
            .NewConfig();

        TypeAdapterConfig<SummaryParseDto, SummaryEntity>.NewConfig();

        TypeAdapterConfig<EducationParseDto, EducationEntity>.NewConfig()
            .Map(dest => dest.StartDate, src => ToDateTimeOrDefault(src.StartDate))
            .Map(dest => dest.EndDate, src => ToNullableDateTime(src.EndDate));

        TypeAdapterConfig<WorkExperienceParseDto, WorkExperienceEntity>.NewConfig()
            .Map(dest => dest.StartDate, src => ToDateTimeOrDefault(src.StartDate))
            .Map(dest => dest.EndDate, src => ToNullableDateTime(src.EndDate));

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
}