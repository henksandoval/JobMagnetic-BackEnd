using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Application.UseCases.CvParser.Mappers;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;

namespace JobMagnet.Unit.Tests.Mappers;

public class ProfileRawMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact(DisplayName = "Map ProfileRaw to ProfileParseDto with direct string properties")]
    public void MapDirectStringPropertiesCorrectly()
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture).Build();

        // --- When ---
        var result = profileRaw.ToProfileParseDto();

        // --- Then ---
        var expectedParseDto = new ProfileParseDto
        {
            FirstName = profileRaw.FirstName,
            LastName = profileRaw.LastName,
            MiddleName = profileRaw.MiddleName,
            SecondLastName = profileRaw.SecondLastName,
            ProfileImageUrl = profileRaw.ProfileImageUrl
        };

        result.Should().NotBeNull();

        result.Should().BeEquivalentTo(expectedParseDto, options => options
            .Including(p => p.FirstName)
            .Including(p => p.LastName)
            .Including(p => p.MiddleName)
            .Including(p => p.SecondLastName)
            .Including(p => p.ProfileImageUrl)
        );
    }

    [Theory(DisplayName = "Map valid BirthDate strings to DateOnly")]
    [InlineData("1990-07-15", 1990, 7, 15)]
    [InlineData("15/07/1990", 1990, 7, 15)]
    [InlineData("July 15, 1990", 1990, 7, 15)]
    [InlineData("1990-07", 1990, 7, 1)]
    [InlineData("07/1990", 1990, 7, 1)]
    [InlineData("1990", 1990, 1, 1)]
    public void ParseValidBirthDateFormatsCorrectly(string rawDateString, int year, int month, int day)
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture)
            .WithBirthDate(rawDateString)
            .Build();

        // --- When ---
        var result = profileRaw.ToProfileParseDto();

        // --- Then ---
        var expectedDate = new DateOnly(year, month, day);
        var expectedParseDto = new ProfileParseDto { BirthDate = expectedDate };

        result.Should().BeEquivalentTo(expectedParseDto, options => options.Including(p => p.BirthDate));
    }


    [Theory(DisplayName = "Map invalid or missing BirthDate string to null DateOnly")]
    [InlineData("          ")]
    [InlineData("")]
    [InlineData(null)]
    public void MapInvalidOrMissingBirthDateToNull(string? rawDateString)
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture)
            .WithBirthDate(rawDateString)
            .Build();

        // --- When ---
        var result = profileRaw.ToProfileParseDto();

        // --- Then ---
        result.BirthDate.Should().BeNull();
    }

    [Fact(DisplayName = "Throw a exception when BirthDate is invalid")]
    public void ThrowWhenBirthDateIsInvalid()
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture)
            .WithBirthDate("invalid-date-string")
            .Build();

        // --- When ---
        var act = () => profileRaw.ToProfileParseDto();

        // --- Then ---
        act.Should().Throw<FormatException>();
    }

    [Fact(DisplayName = "Map ResumeRaw to ResumeParseDto")]
    public void MapResumeRawCorrectly()
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture)
            .WithResume()
            .WithContactInfo()
            .Build();

        // --- When ---
        var result = profileRaw.ToProfileParseDto();

        // --- Then ---
        result.Resume.Should().NotBeNull();

        var expectedResumeDto = new ResumeParseDto
        {
            JobTitle = profileRaw.Resume!.JobTitle,
            About = profileRaw.Resume.About,
            Summary = profileRaw.Resume.Summary,
            Overview = profileRaw.Resume.Overview,
            Title = profileRaw.Resume.Title,
            Suffix = profileRaw.Resume.Suffix,
            Address = profileRaw.Resume.Address,
            ContactInfo = profileRaw.Resume.ContactInfo
                .Select(inf => new ContactInfoParseDto { ContactType = inf.ContactType, Value = inf.Value })
                .ToList()
        };

        result.Resume.Should().BeEquivalentTo(expectedResumeDto);
    }

    [Fact(DisplayName = "Map ResumeRaw when ContactInfo list is null")]
    public void MapResumeRawCorrectlyWhenContactInfoListIsEmpty()
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture)
            .WithResume()
            .Build();

        // --- When ---
        var result = profileRaw.ToProfileParseDto();

        // --- Then ---
        result.Resume.Should().NotBeNull();
        result.Resume.ContactInfo.Should().NotBeNull().And.BeEmpty();
    }

    [Fact(DisplayName = "Map SkillRaw to SkillParseDto")]
    public void MapSkillRawCorrectly()
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture)
            .WithSkillSet()
            .Build();

        // --- When ---
        var result = profileRaw.ToProfileParseDto();

        // --- Then ---
        result.SkillSet.Should().NotBeNull();
        var expectedSkillDto = new SkillSetParseDto
        {
            Overview = profileRaw.SkillSet!.Overview,
            Skills = profileRaw.SkillSet.Skills!
                .Select(skill => new SkillParseDto { Name = skill.Name, Level = Convert.ToUInt16(skill.Level) })
                .ToList()
        };
        result.SkillSet.Should().BeEquivalentTo(expectedSkillDto);
    }

    [Fact(DisplayName = "Map ServiceRaw to ServiceParseDto")]
    public void MapServiceRawCorrectly()
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture)
            .WithServices()
            .Build();

        // --- When ---
        var result = profileRaw.ToProfileParseDto();

        // --- Then ---
        result.Services.Should().NotBeNull();
        var expectedServiceDto = new ServiceParseDto
        {
            Overview = profileRaw.Services!.Overview,
            GalleryItems = profileRaw.Services.GalleryItems!
                .Select(x => new GalleryItemParseDto
                {
                    Title = x.Title,
                    Type = x.Type,
                    Description = x.Description,
                    UrlImage = x.UrlImage,
                    UrlVideo = x.UrlVideo,
                    UrlLink = x.UrlLink
                })
                .ToList()
        };
        result.Services.Should().BeEquivalentTo(expectedServiceDto);
    }

    [Fact(DisplayName = "Map SummaryRaw (Introduction, Education, WorkExperience) to SummaryParseDto")]
    public void MapSummaryRawCorrectly()
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture)
            .WithSummaries()
            .WithEducation()
            .WithWorkExperience()
            .Build();

        // --- When ---
        var result = profileRaw.ToProfileParseDto();

        // --- Then ---
        result.Summary.Should().NotBeNull();
        var expectedSummaryDto = new SummaryParseDto
        {
            Introduction = profileRaw.Summary!.Introduction,
            Education = profileRaw.Summary.Education!
                .Select(education => new EducationParseDto
                {
                    InstitutionName = education.InstitutionName,
                    InstitutionLocation = education.InstitutionLocation,
                    Degree = education.Degree,
                    Description = education.Description,
                    StartDate = DateOnly.Parse(education.StartDate!),
                    EndDate = string.IsNullOrEmpty(education.EndDate) ? null : DateOnly.Parse(education.EndDate!),
                }).ToList(),
            WorkExperiences = profileRaw.Summary.WorkExperiences!
                .Select(work => new WorkExperienceParseDto
                {
                    JobTitle = work.JobTitle,
                    CompanyName = work.CompanyName,
                    CompanyLocation = work.CompanyLocation,
                    Description = work.Description,
                    StartDate = DateOnly.Parse(work.StartDate!),
                    EndDate = string.IsNullOrEmpty(work.EndDate) ? null : DateOnly.Parse(work.EndDate!),
                    Responsibilities = work.Responsibilities!
                        .Select(responsibility => new ResponsibilityParseDto
                        {
                            Description = responsibility.Description
                        }).ToList()
                }).ToList()
        };
        result.Summary.Should().BeEquivalentTo(expectedSummaryDto);
    }

    [Fact(DisplayName = "Map TalentRaw list to TalentParseDto list")]
    public void MapTalentListCorrectly()
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture)
            .WithTalents()
            .Build();

        // --- When ---
        var result = profileRaw.ToProfileParseDto();

        // --- Then ---
        var expectedTalents = profileRaw.Talents
            .Select(talent => new TalentParseDto { Description = talent.Description })
            .ToList();
        result.Talents.Should().BeEquivalentTo(expectedTalents);
    }

    [Fact(DisplayName = "Map PortfolioGalleryRaw list to PortfolioGalleryParseDto list")]
    public void MapPortfolioGalleryListCorrectly()
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture)
            .WithPortfolio()
            .Build();

        // --- When ---
        var result = profileRaw.ToProfileParseDto();

        // --- Then ---
        var expectedPortfolio = profileRaw.PortfolioGallery
            .Select(gallery => new PortfolioGalleryParseDto
            {
                Title = gallery.Title,
                Description = gallery.Description,
                UrlLink = gallery.UrlLink,
                UrlImage = gallery.UrlImage,
                UrlVideo = gallery.UrlVideo,
                Type = gallery.Type
            }).ToList();
        result.PortfolioGallery.Should().BeEquivalentTo(expectedPortfolio);
    }

    [Fact(DisplayName = "Map TestimonialRaw list to TestimonialParseDto list")]
    public void MapTestimonialListCorrectly()
    {
        // --- Given ---
        var profileRaw = new ProfileRawBuilder(_fixture)
            .WithTestimonials()
            .Build();

        // --- When ---
        var result = profileRaw.ToProfileParseDto();

        // --- Then ---
        var expectedTestimonials = profileRaw.Testimonials!
            .Select(testimonial => new TestimonialParseDto
            {
                Name = testimonial.Name,
                JobTitle = testimonial.JobTitle,
                PhotoUrl = testimonial.PhotoUrl,
                Feedback = testimonial.Feedback
            }).ToList();
        result.Testimonials.Should().BeEquivalentTo(expectedTestimonials);
    }
}