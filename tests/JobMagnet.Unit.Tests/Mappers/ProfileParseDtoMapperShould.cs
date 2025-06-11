using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Application.UseCases.CvParser.Mappers;
using JobMagnet.Domain.Core.Entities;
using Shouldly;

namespace JobMagnet.Unit.Tests.Mappers;

public class ProfileParseDtoMapperShould
{
    [Fact(DisplayName = "Map ProfileParseDto to ProfileEntity - Basic Info")]
    public void MapProfileParseDtoToProfileEntity_BasicInfo()
    {
        // Given
        var dto = new ProfileParseDto
        {
            FirstName = "Peter",
            LastName = "Pan",
            ProfileImageUrl = "peter.jpg",
            BirthDate = new DateOnly(2000, 5, 10),
            MiddleName = "W.",
            SecondLastName = "Lost"
        };

        // When
        var entity = dto.ToProfileEntity();

        // Then
        entity.ShouldNotBeNull();
        entity.FirstName.ShouldBe(dto.FirstName);
        entity.LastName.ShouldBe(dto.LastName);
        entity.ProfileImageUrl.ShouldBe(dto.ProfileImageUrl);
        entity.BirthDate.ShouldBe(dto.BirthDate);
        entity.MiddleName.ShouldBe(dto.MiddleName);
        entity.SecondLastName.ShouldBe(dto.SecondLastName);
    }

    [Fact(DisplayName = "Map ProfileParseDto to ProfileEntity - With Resume")]
    public void MapProfileParseDtoToProfileEntity_WithResume()
    {
        // Given
        var dto = CreateFullProfileParseDto();
        dto.Skill = null;
        dto.Services = null;

        // When
        var entity = dto.ToProfileEntity();

        // Then
        var expectedResume = new ResumeEntity
        {
            Id = 0,
            JobTitle = dto.Resume!.JobTitle!,
            About = dto.Resume.About!,
            Summary = dto.Resume.Summary!,
            Overview = dto.Resume.Overview!,
            Title = dto.Resume.Title,
            Suffix = dto.Resume.Suffix,
            Address = dto.Resume.Address,
            ContactInfo = dto.Resume.ContactInfo.Select(inf => new ContactInfoEntity
            {
                Id = 0,
                Value = inf.Value!,
                ContactType = new ContactTypeEntity(inf.ContactType)
            }).ToList()
        };
        entity.ShouldNotBeNull();
        entity.Resume.ShouldBeEquivalentTo(expectedResume);
    }

    [Fact(DisplayName = "Map ProfileParseDto to ProfileEntity - With Skills")]
    public void MapProfileParseDtoToProfileEntity_WithSkills()
    {
        // Given
        var dto = CreateFullProfileParseDto();

        // When
        var entity = dto.ToProfileEntity();

        // Then
        entity.ShouldNotBeNull();
        entity.Skill.ShouldNotBeNull();
        entity.Skill.Overview.ShouldBe(dto.Skill.Overview);
        entity.Skill.SkillDetails.ShouldNotBeNull();
        entity.Skill.SkillDetails.Count.ShouldBe(dto.Skill.SkillDetails.Count());

        var firstSkillDetailDto = dto.Skill.SkillDetails.First();
        var firstSkillDetailEntity = entity.Skill.SkillDetails.First();
        firstSkillDetailEntity.Name.ShouldBe(firstSkillDetailDto.Name);
        firstSkillDetailEntity.ProficiencyLevel.ShouldBe(firstSkillDetailDto.Level ?? 0);
        // Category, Rank, IconUrl tendrán valores por defecto ya que no están en el DTO.
    }

    [Fact(DisplayName = "Map ProfileParseDto to ProfileEntity - With Services")]
    public void MapProfileParseDtoToProfileEntity_WithServices()
    {
        // Given
        var dto = CreateFullProfileParseDto();

        // When
        var entity = dto.ToProfileEntity();

        // Then
        entity.ShouldNotBeNull();
        entity.Services.ShouldNotBeNull();
        entity.Services.Overview.ShouldBe(dto.Services.Overview);
        entity.Services.GalleryItems.ShouldNotBeNull();
        entity.Services.GalleryItems.Count.ShouldBe(dto.Services.GalleryItems.Count());

        var firstGalleryItemDto = dto.Services.GalleryItems.First();
        var firstGalleryItemEntity = entity.Services.GalleryItems.First();
        firstGalleryItemEntity.Title.ShouldBe(firstGalleryItemDto.Title);
        firstGalleryItemEntity.Description.ShouldBe(firstGalleryItemDto.Description);
    }

    [Fact(DisplayName = "Map ProfileParseDto to ProfileEntity - With Summary (Education & WorkExperience)", Skip = "Temp skip")]
    public void MapProfileParseDtoToProfileEntity_WithSummary()
    {
        // Given
        var dto = CreateFullProfileParseDto();

        // When
        var entity = dto.ToProfileEntity();

        // Then
        entity.ShouldNotBeNull();
        entity.Summary.ShouldNotBeNull();
        entity.Summary.Introduction.ShouldBe(dto.Summary.Introduction);

        // Education
        entity.Summary.Education.ShouldNotBeNull();
        entity.Summary.Education.Count.ShouldBe(dto.Summary.Education.Count());
        var firstEduDto = dto.Summary.Education.First();
        var firstEduEntity = entity.Summary.Education.First();
        firstEduEntity.Degree.ShouldBe(firstEduDto.Degree);
        firstEduEntity.InstitutionName.ShouldBe(firstEduDto.InstitutionName);
        firstEduEntity.StartDate.ShouldBe(firstEduDto.StartDate.HasValue
            ? firstEduDto.StartDate.Value.ToDateTime(TimeOnly.MinValue)
            : default(DateTime));
        firstEduEntity.EndDate.ShouldBe(firstEduDto.EndDate.HasValue
            ? (DateTime?)firstEduDto.EndDate.Value.ToDateTime(TimeOnly.MinValue)
            : null);


        // Work Experience
        entity.Summary.WorkExperiences.ShouldNotBeNull();
        entity.Summary.WorkExperiences.Count.ShouldBe(dto.Summary.WorkExperiences.Count());
        var firstWorkDto = dto.Summary.WorkExperiences.First();
        var firstWorkEntity = entity.Summary.WorkExperiences.First();
        firstWorkEntity.JobTitle.ShouldBe(firstWorkDto.JobTitle);
        firstWorkEntity.CompanyName.ShouldBe(firstWorkDto.CompanyName);
        // Responsibilities será una lista vacía ya que no está en el DTO.
        firstWorkEntity.Responsibilities.ShouldNotBeNull();
        firstWorkEntity.Responsibilities.ShouldBeEmpty();
    }


    [Fact(DisplayName = "Map ProfileParseDto to ProfileEntity - With Talents")]
    public void MapProfileParseDtoToProfileEntity_WithTalents()
    {
        // Given
        var dto = CreateFullProfileParseDto();

        // When
        var entity = dto.ToProfileEntity();

        // Then
        entity.ShouldNotBeNull();
        entity.Talents.ShouldNotBeNull();
        entity.Talents.Count.ShouldBe(dto.Talents.Count);
        entity.Talents.First().Description.ShouldBe(dto.Talents.First().Description);
    }

    [Fact(DisplayName = "Map ProfileParseDto to ProfileEntity - With PortfolioGallery")]
    public void MapProfileParseDtoToProfileEntity_WithPortfolioGallery()
    {
        // Given
        var dto = CreateFullProfileParseDto();

        // When
        var entity = dto.ToProfileEntity();

        // Then
        entity.ShouldNotBeNull();
        entity.PortfolioGallery.ShouldNotBeNull();
        entity.PortfolioGallery.Count.ShouldBe(dto.PortfolioGallery.Count);
        entity.PortfolioGallery.First().Title.ShouldBe(dto.PortfolioGallery.First().Title);
    }

    [Fact(DisplayName = "Map ProfileParseDto to ProfileEntity - With Testimonials")]
    public void MapProfileParseDtoToProfileEntity_WithTestimonials()
    {
        // Given
        var dto = CreateFullProfileParseDto();

        // When
        var entity = dto.ToProfileEntity();

        // Then
        entity.ShouldNotBeNull();
        entity.Testimonials.ShouldNotBeNull();
        entity.Testimonials.Count.ShouldBe(dto.Testimonials.Count);
        var firstTestimonialEntity = entity.Testimonials.First();
        var firstTestimonialDto = dto.Testimonials.First();
        firstTestimonialEntity.Name.ShouldBe(firstTestimonialDto.Name);
        firstTestimonialEntity.JobTitle.ShouldBe(firstTestimonialDto.JobTitle);
        firstTestimonialEntity.Feedback.ShouldBe(firstTestimonialDto.Feedback);
    }

    [Fact(DisplayName = "Map ProfileParseDto to ProfileEntity - Handles Null DTO Properties Gracefully")]
    public void MapProfileParseDtoToProfileEntity_HandlesNulls()
    {
        // Given
        var dto = new ProfileParseDto
        {
            FirstName = "OnlyFirstName",
            Resume = new ResumeParseDto
            {
                ContactInfo = []
            },
            Skill = new SkillParseDto
            {
                SkillDetails = []
            }
        };

        // When
        var entity = dto.ToProfileEntity();

        // Then
        entity.ShouldNotBeNull();
        entity.FirstName.ShouldBe("OnlyFirstName");
        entity.LastName.ShouldBeNull();

        entity.Resume.ShouldNotBeNull();
        entity.Resume.ContactInfo.ShouldNotBeNull();
        entity.Resume.ContactInfo.ShouldBeEmpty();

        entity.Skill.ShouldNotBeNull();
        entity.Skill.SkillDetails.ShouldNotBeNull();
        entity.Skill.SkillDetails.ShouldBeEmpty();

        entity.Services.ShouldBeNull();
        entity.Summary.ShouldBeNull();
        entity.Talents.ShouldNotBeNull();
        entity.Talents.ShouldBeEmpty();
        entity.PortfolioGallery.ShouldNotBeNull();
        entity.PortfolioGallery.ShouldBeEmpty();
        entity.Testimonials.ShouldNotBeNull();
        entity.Testimonials.ShouldBeEmpty();
    }

    [Fact(DisplayName = "Map ProfileParseDto with empty collections to ProfileEntity with empty collections")]
    public void MapProfileParseDto_EmptyCollections_ToProfileEntity_EmptyCollections()
    {
        // Given
        var dto = new ProfileParseDto
        {
            FirstName = "Test",
            Talents = [],
            PortfolioGallery = [],
            Testimonials = [],
            Resume = new ResumeParseDto { ContactInfo = new List<ContactInfoParseDto>() },
            Skill = new SkillParseDto { SkillDetails = new List<SkillDetailParseDto>() },
            Services = new ServiceParseDto { GalleryItems = new List<GalleryItemParseDto>() },
            Summary = new SummaryParseDto
            {
                Education = new List<EducationParseDto>(),
                WorkExperiences = new List<WorkExperienceParseDto>()
            }
        };

        // When
        var entity = dto.ToProfileEntity();

        // Then
        entity.ShouldNotBeNull();
        entity.Talents.ShouldBeEmpty();
        entity.PortfolioGallery.ShouldBeEmpty();
        entity.Testimonials.ShouldBeEmpty();
        entity.Resume.ContactInfo.ShouldBeEmpty();
        entity.Skill.SkillDetails.ShouldBeEmpty();
        entity.Services.GalleryItems.ShouldBeEmpty();
        entity.Summary.Education.ShouldBeEmpty();
        entity.Summary.WorkExperiences.ShouldBeEmpty();
    }

    private static ProfileParseDto CreateFullProfileParseDto()
    {
        return new ProfileParseDto
        {
            FirstName = "John",
            LastName = "Doe",
            MiddleName = "M.",
            SecondLastName = "Smith",
            ProfileImageUrl = "https://example.com/profile.jpg",
            BirthDate = new DateOnly(1990, 1, 15),
            Resume = new ResumeParseDto
            {
                JobTitle = "Software Developer",
                About = "Experienced software developer.",
                Summary = "A summary of my career.",
                Overview = "An overview of my skills.",
                Title = "Mr.",
                Suffix = "Jr.",
                Address = "123 Main St, Anytown",
                ContactInfo = new List<ContactInfoParseDto>
                {
                    new() { ContactType = "Email", Value = "john.doe@example.com" },
                    new() { ContactType = "Phone", Value = "555-1234" }
                }
            },
            Skill = new SkillParseDto
            {
                Overview = "Technical Skills Overview",
                SkillDetails = new List<SkillDetailParseDto>
                {
                    new() { Name = "C#", Level = 5 },
                    new() { Name = "SQL", Level = 4 }
                }
            },
            Services = new ServiceParseDto
            {
                Overview = "Services Offered Overview",
                GalleryItems = new List<GalleryItemParseDto>
                {
                    new() { Title = "Web Development", Description = "Full-stack web dev", UrlImage = "img1.jpg" }
                }
            },
            Summary = new SummaryParseDto
            {
                Introduction = "A brief introduction.",
                Education = new List<EducationParseDto>
                {
                    new()
                    {
                        Degree = "BSc Computer Science", InstitutionName = "Tech University",
                        StartDate = new DateOnly(2010, 9, 1), EndDate = new DateOnly(2014, 6, 1),
                        Description = "Studied CS."
                    }
                },
                WorkExperiences = new List<WorkExperienceParseDto>
                {
                    new()
                    {
                        JobTitle = "Junior Developer", CompanyName = "Old Startup",
                        StartDate = new DateOnly(2014, 7, 1), EndDate = new DateOnly(2016, 12, 31),
                        Description = "Worked on various projects."
                    }
                }
            },
            Talents =
            [
                new() { Description = "Problem Solving" },
                new() { Description = "Teamwork" }
            ],
            PortfolioGallery =
            [
                new() { Title = "Project Alpha", Description = "A cool project", UrlLink = "https://example.com/alpha" }
            ],
            Testimonials = [new() { Name = "Jane Smith", JobTitle = "Manager", Feedback = "Great to work with!" }]
        };
    }
}