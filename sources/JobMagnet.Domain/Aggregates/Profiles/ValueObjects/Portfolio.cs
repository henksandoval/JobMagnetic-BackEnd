using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public class Portfolio
{
    private readonly Profile _profile;
    private IReadOnlyCollection<Project> Projects => _profile.Projects;

    private Portfolio()
    {
    }

    internal Portfolio(Profile profile)
    {
        Guard.IsNotNull(profile);
        _profile = profile;
    }

    public Project AddProject(IGuidGenerator guidGenerator, IClock clock, string title, string description, string urlLink, string urlImage, string urlVideo, string type)
    {
        if (Projects.Count >= 20) throw new JobMagnetDomainException("Cannot add more than 20 testimonials.");

        var position = Projects.Count > 0 ? Projects.Max(x => x.Position) + 1 : 1;
        var project = Project.CreateInstance(
            guidGenerator,
            clock,
            _profile.Id,
            title,
            description,
            urlLink,
            urlImage,
            urlVideo,
            type,
            position);

        _profile.AddProjectToPortfolio(project);

        return project;
    }
}