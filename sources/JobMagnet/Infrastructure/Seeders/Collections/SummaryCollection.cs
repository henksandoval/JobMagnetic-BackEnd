using System.Collections.Immutable;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record SummaryCollection
{
    private readonly long _summaryId;
    private record EducationProperties(
        string Degree,
        string InstitutionName,
        string InstitutionLocation,
        DateTime StartDate,
        DateTime? EndDate,
        string Description
    );

    private readonly IList<EducationProperties> _values =
    [
       new ("Bachelor in UI/UX Design",
            "University of California, Berkeley",
            "Berkeley, CA",
            new DateTime(2010, 1, 1),
            new DateTime(2014, 1, 1),
            "Bachelor's degree in UI/UX Design with a focus on user-centered design principles."),
        new ("Master in Graphic Design",
            "California Institute of the Arts",
            "Valencia, CA",
            new DateTime(2014, 1, 1),
            new DateTime(2016, 1, 1),
            "Master's degree in Graphic Design with a focus on visual communication and branding."),
      new ("PhD in Human-Computer Interaction",
            "Stanford University",
            "Stanford, CA",
            new DateTime(2016, 1, 1),
            new DateTime(2020, 1, 1),
            "PhD in Human-Computer Interaction with a focus on user experience research and design."),
        new ("Certificate in Web Development",
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
}