using CommunityToolkit.Diagnostics;
using CSharpFunctionalExtensions;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Utils;

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
        if (Projects.Count >= 20) throw new JobMagnetDomainException("Cannot add more than 20 projects.");

        var position = GetPosition();
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

    public Project UpdateProject(
        ProjectId projectId,
        string newTitle,
        string newDescription,
        string newUrlLink,
        string newUrlImage,
        string newUrlVideo,
        string newType)
    {
        var projectToUpdate = Projects.FirstOrDefault(p => p.Id == projectId);

        if (projectToUpdate is null)
            throw NotFoundException.For<Project, ProjectId>(projectId);

        if (Projects.Any(p => p.Id != projectId && p.Title == newTitle))
            throw new JobMagnetDomainException("A project with this title already exists in the portfolio.");

        return projectToUpdate.UpdateDetails(newTitle, newDescription, newUrlLink, newUrlImage, newUrlVideo, newType);
    }

    public Project RemoveProject(ProjectId projectId)
    {
        var projectToRemove = Projects.FirstOrDefault(p => p.Id == projectId);

        if (projectToRemove is null)
            throw NotFoundException.For<Project, ProjectId>(projectId);

        _profile.RemoveProjectToPortfolio(projectToRemove);
        return projectToRemove;
    }

    public void ArrangeProjects(IEnumerable<ProjectId> orderedProjects)
    {
        var projectIds = orderedProjects.ToList();
        Guard.IsNotNull(projectIds);

        var currentProjectIds = new HashSet<ProjectId>(Projects.Select(p => p.Id));

        if (projectIds.Count != new HashSet<ProjectId>(projectIds).Count)
            throw new BusinessRuleValidationException("The list of project IDs for reordering contains duplicates.");

        if (!currentProjectIds.SetEquals(projectIds))
            throw new BusinessRuleValidationException("The provided project list for reordering does not match the projects in the portfolio. Ensure all projects are included exactly once.");

        foreach (var (projectId, index) in projectIds.WithIndex())
        {
            var position = index + 1;
            var projectToUpdate = Projects.Single(p => p.Id == projectId);
            projectToUpdate.UpdatePosition(position);
        }
    }

    private int GetPosition()
    {
        return Projects.Count > 0 ? Projects.Max(x => x.Position) + 1 : 1;
    }
}