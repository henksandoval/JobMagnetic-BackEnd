using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public class CareerHistory : SoftDeletableEntity<CareerHistoryId>
{
    private readonly HashSet<Qualification> _qualifications = [];
    private readonly HashSet<WorkExperience> _workExperiences = [];

    public string Introduction { get; private set; }
    public ProfileId ProfileId { get; private set; }
    public virtual IReadOnlyCollection<Qualification> Qualifications => _qualifications;
    public virtual IReadOnlyCollection<WorkExperience> WorkExperiences => _workExperiences;

    private CareerHistory() : base() { }

    private CareerHistory(CareerHistoryId id, string introduction, ProfileId profileId) : base(id)
    {
        Guard.IsNotNullOrWhiteSpace(introduction);

        Id = id;
        ProfileId = profileId;
        Introduction = introduction;
    }

    public static CareerHistory CreateInstance(IGuidGenerator guidGenerator, ProfileId profileId, string introduction)
    {
        var id = new CareerHistoryId(guidGenerator.NewGuid());
        return new CareerHistory(id, introduction, profileId);
    }

    public void UpdateIntroduction(string introduction)
    {
        Guard.IsNotNullOrWhiteSpace(introduction);
        Introduction = introduction;
    }

    public Qualification AddEducation(IGuidGenerator guidGenerator, string degree, string institutionName,
        string institutionLocation, DateTime startDate, DateTime? endDate, string description)
    {
        Guard.IsNotNull(guidGenerator);

        if (_qualifications.Count >= 10)
            throw new BusinessRuleValidationException("Cannot add more than 10 qualifications.");

        if (_qualifications.Any(q => q.Degree == degree && q.InstitutionName == institutionName))
            throw new BusinessRuleValidationException("A qualification with this degree and institution already exists.");

        var qualification = Qualification.CreateInstance(
            guidGenerator,
            Id,
            degree,
            institutionName,
            institutionLocation,
            startDate,
            endDate,
            description);

        _qualifications.Add(qualification);
        return qualification;
    }

    public void UpdateEducation(QualificationId qualificationId, string degree, string institutionName,
        string institutionLocation, DateTime startDate, DateTime? endDate, string description)
    {
        var qualification = _qualifications.FirstOrDefault(q => q.Id == qualificationId);
        if (qualification is null)
            throw NotFoundException.For<Qualification, QualificationId>(qualificationId);

        if (_qualifications.Any(q => q.Id != qualificationId && q.Degree == degree && q.InstitutionName == institutionName))
            throw new BusinessRuleValidationException("A qualification with this degree and institution already exists.");

        qualification.Update(degree, institutionName, institutionLocation, startDate, endDate, description);
    }

    public void RemoveEducation(QualificationId qualificationId)
    {
        var qualification = _qualifications.FirstOrDefault(q => q.Id == qualificationId);
        if (qualification is null)
            throw NotFoundException.For<Qualification, QualificationId>(qualificationId);

        _qualifications.Remove(qualification);
    }

    public WorkExperience AddWorkExperience(IGuidGenerator guidGenerator, string jobTitle, string companyName,
        string companyLocation, DateTime startDate, DateTime? endDate, string description)
    {
        Guard.IsNotNull(guidGenerator);

        if (_workExperiences.Count >= 15)
            throw new BusinessRuleValidationException("Cannot add more than 15 work experiences.");

        if (_workExperiences.Any(w => w.JobTitle == jobTitle && w.CompanyName == companyName &&
            w.StartDate == startDate))
            throw new BusinessRuleValidationException("A work experience with this job title, company and start date already exists.");

        var workExperience = WorkExperience.CreateInstance(
            guidGenerator,
            Id,
            jobTitle,
            companyName,
            companyLocation,
            startDate,
            endDate,
            description);

        _workExperiences.Add(workExperience);
        return workExperience;
    }

    public void UpdateWorkExperience(WorkExperienceId workExperienceId, string jobTitle, string companyName,
        string companyLocation, DateTime startDate, DateTime? endDate, string description)
    {
        var workExperience = _workExperiences.FirstOrDefault(w => w.Id == workExperienceId);
        if (workExperience is null)
            throw NotFoundException.For<WorkExperience, WorkExperienceId>(workExperienceId);

        if (_workExperiences.Any(w => w.Id != workExperienceId && w.JobTitle == jobTitle &&
            w.CompanyName == companyName && w.StartDate == startDate))
            throw new BusinessRuleValidationException("A work experience with this job title, company and start date already exists.");

        workExperience.Update(jobTitle, companyName, companyLocation, startDate, endDate, description);
    }

    public void RemoveWorkExperience(WorkExperienceId workExperienceId)
    {
        var workExperience = _workExperiences.FirstOrDefault(w => w.Id == workExperienceId);
        if (workExperience is null)
            throw NotFoundException.For<WorkExperience, WorkExperienceId>(workExperienceId);

        _workExperiences.Remove(workExperience);
    }
}