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

        TypeAdapterConfig<ResumeParseDto, ResumeEntity>
            .NewConfig()
            .Map(dest => dest.About, src => src.About ?? string.Empty)
            .Map(dest => dest.Overview, src => src.Overview ?? string.Empty)
            .Map(dest => dest.Summary, src => src.Summary ?? string.Empty);

        TypeAdapterConfig<ContactInfoParseDto, ContactInfoEntity>.NewConfig()
            .Map(dest => dest.ContactType, src => new ContactTypeEntity
            {
                Id = 0,
                Name = src.ContactType ?? string.Empty
            });

        TypeAdapterConfig<SkillParseDto, SkillEntity>.NewConfig();

        TypeAdapterConfig<SkillDetailParseDto, SkillItemEntity>.NewConfig()
            .Map(dest => dest.ProficiencyLevel, src => src.Level)
            .Map(dest => dest.IconUrl, src => string.Empty)
            .Map(dest => dest.Category, src => string.Empty)
            .Map(dest => dest.Rank, src => 0);

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
}