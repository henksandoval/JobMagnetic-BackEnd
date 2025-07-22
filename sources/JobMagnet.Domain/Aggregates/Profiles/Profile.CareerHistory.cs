using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles;

public partial class Profile
{
    public IReadOnlyCollection<AcademicDegree> AcademicDegreesInCareerHistory => CareerHistory?.AcademicDegree ?? [];
    public IReadOnlyCollection<WorkExperience> WorkExperiencesInCareerHistory => CareerHistory?.WorkExperiences ?? [];

    public void AddCareerHistory(CareerHistory careerHistory)
    {
        Guard.IsNotNull(careerHistory);

        CareerHistory = careerHistory;
    }

    public void CreateCareerHistory(IGuidGenerator guidGenerator, string introduction)
    {
        if (CareerHistory is not null)
            throw new BusinessRuleValidationException("Career history already exists for this profile.");

        var careerHistory = CareerHistory.CreateInstance(guidGenerator, Id, introduction);
        AddCareerHistory(careerHistory);
    }

    public void UpdateCareerHistoryIntroduction(string introduction)
    {
        if (CareerHistory is null)
            throw NotFoundException.For<CareerHistory, ProfileId>(Id);

        CareerHistory.UpdateIntroduction(introduction);
    }

    public void RemoveCareerHistory()
    {
        if (CareerHistory is null)
            throw NotFoundException.For<CareerHistory, ProfileId>(Id);

        CareerHistory = null;
    }

    public bool AcademicDegreeExistsInCareerHistory(string degree, string institutionName) =>
        CareerHistory!.AcademicDegreeExists(degree, institutionName);

    public AcademicDegree AddAcademicDegreeToCareerHistory(IGuidGenerator guidGenerator, string degree,
        string institutionName, string institutionLocation, DateTime startDate, DateTime? endDate, string description)
    {
        if (CareerHistory is null)
            throw NotFoundException.For<CareerHistory, ProfileId>(Id);

        return CareerHistory.AddAcademicDegree(guidGenerator, degree, institutionName, institutionLocation,
            startDate, endDate, description);
    }

    public void UpdateAcademicDegreeInCareerHistory(AcademicDegreeId academicDegreeId, string degree,
        string institutionName, string institutionLocation, DateTime startDate, DateTime? endDate, string description)
    {
        if (CareerHistory is null)
            throw NotFoundException.For<CareerHistory, ProfileId>(Id);

        CareerHistory.UpdateAcademicDegree(academicDegreeId, degree, institutionName, institutionLocation,
            startDate, endDate, description);
    }

    public void RemoveAcademicDegreeFromCareerHistory(AcademicDegreeId academicDegreeId)
    {
        if (CareerHistory is null)
            throw NotFoundException.For<CareerHistory, ProfileId>(Id);

        CareerHistory.RemoveAcademicDegree(academicDegreeId);
    }

    public bool WorkExperienceExists(string jobTitle, string companyName, DateTime startDate) =>
        CareerHistory!.WorkExperienceExists(jobTitle, companyName, startDate);

    public WorkExperience AddWorkExperienceToCareerHistory(IGuidGenerator guidGenerator, string jobTitle,
        string companyName, string companyLocation, DateTime startDate, DateTime? endDate, string description)
    {
        if (CareerHistory is null)
            throw NotFoundException.For<CareerHistory, ProfileId>(Id);

        return CareerHistory.AddWorkExperience(guidGenerator, jobTitle, companyName, companyLocation,
            startDate, endDate, description);
    }

    public void UpdateWorkExperienceInCareerHistory(WorkExperienceId workExperienceId, string jobTitle,
        string companyName, string companyLocation, DateTime startDate, DateTime? endDate, string description)
    {
        if (CareerHistory is null)
            throw NotFoundException.For<CareerHistory, ProfileId>(Id);

        CareerHistory.UpdateWorkExperience(workExperienceId, jobTitle, companyName, companyLocation,
            startDate, endDate, description);
    }

    public void RemoveWorkExperienceFromCareerHistory(WorkExperienceId workExperienceId)
    {
        if (CareerHistory is null)
            throw NotFoundException.For<CareerHistory, ProfileId>(Id);

        CareerHistory.RemoveWorkExperience(workExperienceId);
    }
}