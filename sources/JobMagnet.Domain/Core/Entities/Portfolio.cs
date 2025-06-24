using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities;

public class Portfolio
{
    private readonly ProfileEntity _profile ;
    private IReadOnlyCollection<Project> Projects => _profile.Projects;

    internal Portfolio(ProfileEntity profile)
    {
        Guard.IsNotNull(profile);
        _profile = profile;
    }

    public void AddProject(string title, string description, string urlLink, string urlImage, string urlVideo, string type)
    {
        if (Projects.Count >= 20)
        {
            throw new JobMagnetDomainException("Cannot add more than 20 testimonials.");
        }

        var project = new Project( title, description, urlLink, urlImage, urlVideo, type, _profile.Id);

        _profile.AddProjectToPortfolio(project);
    }
}