using System.Collections.Immutable;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Shared.Data;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record ProjectSeeder
{
    private readonly ProfileId _profileId;

    public ProjectSeeder(ProfileId profileId)
    {
        _profileId = profileId;
    }

    public ImmutableList<Project> GetProjects()
    {
        return ProjectRawData.Data.Select((x, index) => Project.CreateInstance(
                x.Title,
                x.Description,
                x.UrlLink,
                x.UrlImage,
                x.UrlVideo ?? string.Empty,
                x.Type,
                ++index,
                _profileId,
                new ProjectId()))
            .ToImmutableList();
    }
}