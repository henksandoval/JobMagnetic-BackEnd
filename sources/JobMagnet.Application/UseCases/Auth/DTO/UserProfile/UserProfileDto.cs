using System.Collections.Generic;

namespace JobMagnet.Application.UseCases.Auth.DTO.UserProfile;

public class UserProfileDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public List<string> Roles { get; set; }
    public List<string> Permissions { get; set; }
    public string? avatarUrl { get; set; }
}