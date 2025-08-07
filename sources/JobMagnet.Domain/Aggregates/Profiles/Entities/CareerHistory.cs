using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public class CareerHistory : SoftDeletableEntity<CareerHistoryId>
{
    private readonly HashSet<AcademicDegree> _academicDegree = [];
    private readonly HashSet<WorkExperience> _workExperiences = [];

    public string Introduction { get; private set; }
    public ProfileId ProfileId { get; private set; }
    public virtual IReadOnlyCollection<AcademicDegree> AcademicDegree => _academicDegree;
    public virtual IReadOnlyCollection<WorkExperience> WorkExperiences => _workExperiences;

    private CareerHistory()
    {
    }

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

    public AcademicDegree AddAcademicDegree(IGuidGenerator guidGenerator, string degree, string institutionName,
        string institutionLocation, DateTime startDate, DateTime? endDate, string description)
    {
        Guard.IsNotNull(guidGenerator);

        if (_academicDegree.Count >= 10)
            throw new BusinessRuleValidationException("Cannot add more than 10 AcademicDegrees.");

        if (AcademicDegreeExists(degree, institutionName))
            throw new BusinessRuleValidationException("A AcademicDegree with this degree and institution already exists.");

        var academicDegree = Entities.AcademicDegree.CreateInstance(
            new CreateAcademicDegreeCommand(
                guidGenerator,
                Id,
                new CreateAcademicDegreeCommand.AcademicInfo(degree, institutionName, institutionLocation, description),
                startDate,
                endDate));
        _academicDegree.Add(academicDegree);
        return academicDegree;
    }

    public bool AcademicDegreeExists(string degree, string institutionName) =>
        _academicDegree.Any(q => q.Degree == degree && q.InstitutionName == institutionName);

    public void UpdateAcademicDegree(AcademicDegreeId academicDegreeId, string degree, string institutionName,
        string institutionLocation, DateTime startDate, DateTime? endDate, string description)
    {
        var academicDegreeToUpdate = _academicDegree.FirstOrDefault(q => q.Id == academicDegreeId);
        if (academicDegreeToUpdate is null)
            throw NotFoundException.For<AcademicDegree, AcademicDegreeId>(academicDegreeId);

        if (_academicDegree.Any(q => q.Id != academicDegreeId && q.Degree == degree && q.InstitutionName == institutionName))
            throw new BusinessRuleValidationException("A AcademicDegree with this degree and institution already exists.");

        academicDegreeToUpdate.Update(degree, institutionName, institutionLocation, startDate, endDate, description);
    }

    public void RemoveAcademicDegree(AcademicDegreeId academicDegreeId)
    {
        var academicDegreeToRemove = _academicDegree.FirstOrDefault(q => q.Id == academicDegreeId);
        if (academicDegreeToRemove is null)
            throw NotFoundException.For<AcademicDegree, AcademicDegreeId>(academicDegreeId);

        _academicDegree.Remove(academicDegreeToRemove);
    }

    public WorkExperience AddWorkExperience(IGuidGenerator guidGenerator, string jobTitle, string companyName,
        string companyLocation, DateTime startDate, DateTime? endDate, string description)
    {
        Guard.IsNotNull(guidGenerator);

        if (_workExperiences.Count >= 15)
            throw new BusinessRuleValidationException("Cannot add more than 15 work experiences.");

        if (WorkExperienceExists(jobTitle, companyName, startDate))
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

    public bool WorkExperienceExists(string jobTitle, string companyName, DateTime startDate) =>
        _workExperiences.Any(w => w.JobTitle == jobTitle && w.CompanyName == companyName && w.StartDate == startDate);

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