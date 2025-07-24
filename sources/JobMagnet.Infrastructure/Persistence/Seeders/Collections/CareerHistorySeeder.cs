using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record CareerHistorySeeder(IGuidGenerator GuidGenerator, IClock Clock, CareerHistoryId CareerHistoryId)
{
    private static readonly IList<WorkExperienceProperties> WorkExperienceList =
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
            new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Utc),
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
            new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc),
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
            new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            null,
            "Worked as a senior UI/UX designer at Facebook, focusing on user experience research and design.")
    ];

    private static readonly IList<EducationProperties> AcademicDegreeList =
    [
        new("Bachelor in UI/UX Design",
            "University of California, Berkeley",
            "Berkeley, CA",
            new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            "Bachelor's degree in UI/UX Design with a focus on user-centered design principles."),
        new("Master in Graphic Design",
            "California Institute of the Arts",
            "Valencia, CA",
            new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            "Master's degree in Graphic Design with a focus on visual communication and branding."),
        new("PhD in Human-Computer Interaction",
            "Stanford University",
            "Stanford, CA",
            new DateTime(2016, 1, 1,0, 0, 0, DateTimeKind.Utc),
            new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            "PhD in Human-Computer Interaction with a focus on user experience research and design."),
        new("Certificate in Web Development",
            "Codecademy",
            "Online",
            new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            null,
            "Certificate in Web Development with a focus on front-end development and responsive design.")
    ];

    public static int WorkExperienceCount => WorkExperienceList.Count;
    public static int AcademicDegreeCount => AcademicDegreeList.Count;

    public AcademicDegree[] GetAcademicDegrees()
    {
        return AcademicDegreeList
            .Select(x => AcademicDegree.CreateInstance(
                new CreateAcademicDegreeCommand(
                    GuidGenerator,
                    CareerHistoryId,
                    new CreateAcademicDegreeCommand.AcademicInfo(
                        x.Degree,
                        x.InstitutionName,
                        x.InstitutionLocation,
                        x.Description
                    ),
                    x.StartDate,
                    x.EndDate
                )
            )).ToArray();
    }

    public WorkExperience[] GetWorkExperience()
    {
        return WorkExperienceList
            .Select(item =>
            {
                var workExperience = WorkExperience.CreateInstance(
                    GuidGenerator,
                    CareerHistoryId,
                    item.JobTitle,
                    item.CompanyName,
                    item.CompanyLocation,
                    item.StartDate,
                    item.EndDate,
                    item.Description
                );

                foreach (var description in item.Responsibilities)
                    workExperience.AddResponsibility(new WorkHighlight(description));

                return workExperience;
            })
            .ToArray();
    }

    private sealed record EducationProperties(
        string Degree,
        string InstitutionName,
        string InstitutionLocation,
        DateTime StartDate,
        DateTime? EndDate,
        string Description
    );

    private sealed record WorkExperienceProperties(
        string JobTitle,
        string CompanyName,
        string CompanyLocation,
        ICollection<string> Responsibilities,
        DateTime StartDate,
        DateTime? EndDate,
        string Description
    );
}