using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Domain.Aggregates.Profiles;

public partial class Profile
{
    private void AddProjectToPortfolio(IGuidGenerator guidGenerator, string title, string description, string urlLink, string urlImage,
        string urlVideo, string type)
    {
        if (Portfolio.Count >= 20) throw new JobMagnetDomainException("Cannot add more than 20 projects.");

        var position = GetPosition();
        var project = Project.CreateInstance(
            guidGenerator,
            Id,
            title,
            description,
            urlLink,
            urlImage,
            urlVideo,
            type,
            position);

        _portfolio.Add(project);
    }

    private void UpdateProjectInPortfolio(ProjectId projectId,
        string newTitle,
        string newDescription,
        string newUrlLink,
        string newUrlImage,
        string newUrlVideo,
        string newType)
    {
        var projectToUpdate = Portfolio.FirstOrDefault(p => p.Id == projectId);

        if (projectToUpdate is null)
            throw NotFoundException.For<Project, ProjectId>(projectId);

        if (Portfolio.Any(p => p.Id != projectId && p.Title == newTitle))
            throw new JobMagnetDomainException("A project with this title already exists in the portfolio.");

        projectToUpdate.UpdateDetails(newTitle, newDescription, newUrlLink, newUrlImage, newUrlVideo, newType);
    }

    private void RemoveProjectToPortfolio(ProjectId projectId)
    {
        var projectToRemove = Portfolio.FirstOrDefault(p => p.Id == projectId);

        if (projectToRemove is null)
            throw NotFoundException.For<Project, ProjectId>(projectId);

        _portfolio.Remove(projectToRemove);
    }

    private void ArrangeProjectsInPortfolio(IEnumerable<ProjectId> orderedProjects)
    {
        var projectIds = orderedProjects.ToList();
        Guard.IsNotNull(projectIds);

        var currentProjectIds = new HashSet<ProjectId>(Portfolio.Select(p => p.Id));

        if (projectIds.Count != new HashSet<ProjectId>(projectIds).Count)
            throw new BusinessRuleValidationException("The list of project IDs for reordering contains duplicates.");

        if (!currentProjectIds.SetEquals(projectIds))
            throw new BusinessRuleValidationException(
                "The provided project list for reordering does not match the projects in the portfolio. Ensure all projects are included exactly once.");

        foreach (var (projectId, index) in projectIds.WithIndex())
        {
            var position = index + 1;
            var projectToUpdate = Portfolio.Single(p => p.Id == projectId);
            projectToUpdate.UpdatePosition(position);
        }
    }

    private int GetPosition()
    {
        return Portfolio.Count > 0 ? Portfolio.Max(x => x.Position) + 1 : 1;
    }
}