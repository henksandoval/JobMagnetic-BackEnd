using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Exceptions;

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

    public void AddProject(string title, string description, string urlLink, string urlImage, string urlVideo, string type)
    {
        if (Projects.Count >= 20) throw new JobMagnetDomainException("Cannot add more than 20 testimonials.");

        var position = Projects.Count > 0 ? Projects.Max(x => x.Position) + 1 : 1;
        var project = new Project(
            title,
            description,
            urlLink,
            urlImage,
            urlVideo,
            type,
            position,
            _profile.Id,
            new ProjectId());

        _profile.AddProjectToPortfolio(project);
    }
}