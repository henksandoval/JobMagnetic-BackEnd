using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record SummaryCollection
{
    private readonly long _summaryId;


    private readonly IList<WorkExperienceProperties> _value =
    [
        new("UI/UX Designer",
            "Google",
            "Mountain View, CA",
            new List<string>
            {
                "Designed user interfaces for Google products.",
                "Conducted user research and usability testing.",
                "Collaborated with cross-functional teams to improve user experience."
            },
            new DateTime(2014, 1, 1),
            new DateTime(2016, 1, 1),
            "Worked as a UI/UX designer at Google, focusing on user-centered design principles."),
        new("Graphic Designer",
            "Apple",
            "Cupertino, CA",
            new List<string>
            {
                "Created visual designs for Apple products.",
                "Worked on branding and marketing materials.",
                "Collaborated with product teams to ensure design consistency."
            },
            new DateTime(2016, 1, 1),
            new DateTime(2018, 1, 1),
            "Worked as a graphic designer at Apple, focusing on visual communication and branding."),
        new("Senior UI/UX Designer",
            "Facebook",
            "Menlo Park, CA",
            new List<string>
            {
                "Led design projects for Facebook products.",
                "Mentored junior designers and provided design feedback.",
                "Conducted user research and usability testing."
            },
            new DateTime(2018, 1, 1),
            null,
            "Worked as a senior UI/UX designer at Facebook, focusing on user experience research and design.")
    ];

    private readonly IList<EducationProperties> _values =
    [
        new("Bachelor in UI/UX Design",
            "University of California, Berkeley",
            "Berkeley, CA",
            new DateTime(2010, 1, 1),
            new DateTime(2014, 1, 1),
            "Bachelor's degree in UI/UX Design with a focus on user-centered design principles."),
        new("Master in Graphic Design",
            "California Institute of the Arts",
            "Valencia, CA",
            new DateTime(2014, 1, 1),
            new DateTime(2016, 1, 1),
            "Master's degree in Graphic Design with a focus on visual communication and branding."),
        new("PhD in Human-Computer Interaction",
            "Stanford University",
            "Stanford, CA",
            new DateTime(2016, 1, 1),
            new DateTime(2020, 1, 1),
            "PhD in Human-Computer Interaction with a focus on user experience research and design."),
        new("Certificate in Web Development",
            "Codecademy",
            "Online",
            new DateTime(2020, 1, 1),
            null,
            "Certificate in Web Development with a focus on front-end development and responsive design.")
    ];

    public SummaryCollection(long summaryId)
    {
        _summaryId = summaryId;
    }

    public EducationEntity[] GetEducation()
    {
        return _values
            .Select(x => new EducationEntity
            {
                Id = 0,
                Degree = x.Degree,
                InstitutionName = x.InstitutionName,
                InstitutionLocation = x.InstitutionLocation,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                SummaryId = _summaryId,
                Description = x.Description,
                AddedAt = DateTime.UtcNow,
                AddedBy = Guid.Empty
            })
            .ToArray();
    }

    public WorkExperienceEntity[] GetWorkExperience()
    {
        return _value
            .Select(x => new WorkExperienceEntity
            {
                Id = 0,
                JobTitle = x.JobTitle,
                CompanyName = x.CompanyName,
                CompanyLocation = x.CompanyLocation,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Responsibilities = x.Responsibilities?.ToList() ?? [],
                SummaryId = _summaryId,
                Description = x.Description,
                AddedAt = DateTime.UtcNow,
                AddedBy = Guid.Empty
            })
            .ToArray();
    }

    private record EducationProperties(
        string Degree,
        string InstitutionName,
        string InstitutionLocation,
        DateTime StartDate,
        DateTime? EndDate,
        string Description
    );

    private record WorkExperienceProperties(
        string JobTitle,
        string CompanyName,
        string CompanyLocation,
        ICollection<string> Responsibilities,
        DateTime StartDate,
        DateTime? EndDate,
        string Description
    );
}