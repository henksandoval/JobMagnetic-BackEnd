﻿namespace JobMagnet.Host.ViewModels.Profile;

public record SocialNetworksViewModel(
    string Type,
    string Value,
    string? IconClass,
    string? IconUrl
);