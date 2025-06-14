﻿using AutoFixture;
using AutoFixture.Dsl;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileEntityBuilder(IFixture fixture)
{
    private string? _firstName;
    private string? _lastName;
    private ResumeEntity _resume = null!;
    private ServiceEntity _services = null!;
    private SkillEntity _skill = null!;
    private SummaryEntity _summary = null!;
    private List<PortfolioGalleryEntity> _portfolio = [];
    private List<TalentEntity> _talents = [];
    private List<TestimonialEntity> _testimonials = [];

    public ProfileEntityBuilder WithResume()
    {
        _resume = fixture.Create<ResumeEntity>();
        return this;
    }

    public ProfileEntityBuilder WithContactInfo(int count = 5)
    {
        if (_resume == null)
        {
            throw new InvalidOperationException("Cannot add contact info without a resume. Call WithResume() first.");
        }

        _resume.ContactInfo = fixture.CreateMany<ContactInfoEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithSkills()
    {
        _skill = fixture.Create<SkillEntity>();
        return this;
    }

    public ProfileEntityBuilder WithSkillDetails(int count = 5)
    {
        if (_skill == null)
        {
            throw new InvalidOperationException("Cannot add contact info without a summary. Call WithSkills() first.");
        }

        _skill.SkillDetails = fixture.CreateMany<SkillItemEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithServices()
    {
        _services = fixture.Create<ServiceEntity>();
        return this;
    }

    public ProfileEntityBuilder WithTalents(int count = 5)
    {
        _talents = fixture.CreateMany<TalentEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithPortfolio(int count = 5)
    {
        _portfolio = fixture.CreateMany<PortfolioGalleryEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithSummary()
    {
        _summary = fixture.Create<SummaryEntity>();
        return this;
    }

    public ProfileEntityBuilder WithEducation(int count = 5)
    {
        if (_summary == null)
        {
            throw new InvalidOperationException("Cannot add contact info without a summary. Call WithSummary() first.");
        }

        _summary.Education = fixture.CreateMany<EducationEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithWorkExperience(int count = 5)
    {
        if (_summary == null)
        {
            throw new InvalidOperationException("Cannot add contact info without a summary. Call WithSummary() first.");
        }

        _summary.WorkExperiences = fixture.CreateMany<WorkExperienceEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithTestimonials(int count = 5)
    {
        _testimonials = fixture.CreateMany<TestimonialEntity>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithName(string? firstName)
    {
        _firstName = firstName;
        return this;
    }

    public ProfileEntityBuilder WithLastName(string? lastName)
    {
        _lastName = lastName;
        return this;
    }

    public ProfileEntity Build()
    {
        var profile = fixture.Create<ProfileEntity>();

        if (_firstName is not null)
        {
            profile.FirstName = _firstName;
        }

        if (_lastName is not null)
        {
            profile.LastName = _lastName;
        }

        if (_resume is not null)
        {
            profile.Resume = _resume;
        }

        if (_services is not null)
        {
            profile.Services = _services;
        }

        if (_summary is not null)
        {
            profile.Summary = _summary;
        }

        if (_skill is not null)
        {
            profile.Skill = _skill;
        }

        if (_testimonials.Count > 0)
        {
            profile.Testimonials = _testimonials;
        }

        if (_portfolio.Count > 0)
        {
            profile.PortfolioGallery = _portfolio;
        }

        if (_talents.Count > 0)
        {
            profile.Talents = _talents;
        }

        return profile;
    }
}