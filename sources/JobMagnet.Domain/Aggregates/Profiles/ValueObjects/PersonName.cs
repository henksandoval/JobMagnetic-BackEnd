using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public partial record PersonName
{
    public const int MaxNameLength = 15;
    public const int MinNameLength = 2;

    public string? FirstName { get; init; }
    public string? MiddleName { get; init; }
    public string? LastName { get; init; }
    public string? SecondLastName { get; init; }

    private PersonName()
    {
    }

    public static PersonName Empty => new(null, null, null, null, false);

    public bool NotEmpty =>
        FirstName is not null ||
        MiddleName is not null ||
        LastName is not null ||
        SecondLastName is not null;

    public PersonName(string? firstName, string? lastName, string? middleName = null, string? secondLastName = null, bool applyValidations = true)
    {
        if (applyValidations)
        {
            FirstName = ValidateNamePart(firstName, nameof(firstName));
            LastName = ValidateNamePart(lastName, nameof(lastName));
            MiddleName = ValidateNamePart(middleName, nameof(middleName));
            SecondLastName = ValidateNamePart(secondLastName, nameof(secondLastName));
        }
        else
        {
            FirstName = firstName?.Trim();
            LastName = lastName?.Trim();
            MiddleName = middleName?.Trim();
            SecondLastName = secondLastName?.Trim();
        }
    }

    public string GetFullName()
    {
        var parts = new[] { FirstName, MiddleName, LastName, SecondLastName }
            .Where(n => !string.IsNullOrWhiteSpace(n));

        return string.Join(" ", parts);
    }

    public string GetDisplayName()
    {
        var firstName = FirstName ?? string.Empty;
        var lastName = LastName ?? string.Empty;
        return $"{firstName} {lastName}".Trim();
    }

    private static string? ValidateNamePart(string? namePart, string paramName)
    {
        if (string.IsNullOrWhiteSpace(namePart))
            return null;

        namePart = namePart.Trim();

        Guard.IsGreaterThanOrEqualTo(namePart.Length, MinNameLength);
        Guard.IsLessThanOrEqualTo(namePart.Length, MaxNameLength);

        if (!OnlyLettersAndSpaces().IsMatch(namePart))
            throw new ArgumentException($"{paramName} contains invalid characters", paramName);

        return namePart;
    }

    [GeneratedRegex(@"^[\p{L}\s\-'\.]+$")]
    private static partial Regex OnlyLettersAndSpaces();
}