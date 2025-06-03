using System.Text.RegularExpressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Domain.Services;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;

namespace JobMagnet.Unit.Tests.Services;

public partial class ProfileIdentifierNameGeneratorShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly ProfileIdentifierNameGenerator _subject = new ();

    [Fact]
    public void GenerateCorrectIdentifierGivenNormalFirstAndLastName()
    {
        // Given
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName("Juan")
            .WithLastName("Sosa")
            .Build();

        // When
        var identifierName = _subject.GenerateIdentifierName(profile);
        var (namePart, suffix) = ExtractIdentifierParts(identifierName);

        // Then
        identifierName.Length.Should().BeLessThanOrEqualTo(20);
        namePart.Should().Be("juan-sosa");
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
        identifierName.Should().MatchRegex("^[a-z0-9-]+$");
        namePart.Should().NotContain("--").And.NotStartWith("-").And.NotEndWith("-");
    }

    [Fact]
    public void GenerateCorrectIdentifierGivenNameContainingSpecialCharsAndAccents()
    {
        // Given
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName("María")
            .WithLastName("Ñíguez")
            .Build();

        // When
        var identifierName = _subject.GenerateIdentifierName(profile);
        var (namePart, suffix) = ExtractIdentifierParts(identifierName);

        // Then
        identifierName.Should().NotBeNullOrEmpty();
        identifierName.Length.Should().BeLessThanOrEqualTo(20);
        namePart.Should().Be("maria-niguez");
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
        identifierName.Should().MatchRegex("^[a-z0-9-]+$");
    }

    [Fact]
    public void GenerateCorrectIdentifierGivenNameContainingSpecialCharsAccentsAndSpaces()
    {
        // Given
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName("José María")
            .WithLastName("López-Ñíguez")
            .Build();

        // When
        var identifierName = _subject.GenerateIdentifierName(profile);
        var (namePart, suffix) = ExtractIdentifierParts(identifierName);

        // Then
        identifierName.Should().NotBeNullOrEmpty();
        identifierName.Length.Should().BeLessThanOrEqualTo(20);
        namePart.Should().Be("jose-lopez");
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
        identifierName.Should().MatchRegex("^[a-z0-9-]+$");
    }

    [Fact]
    public void GenerateCorrectIdentifierGivenLongNameAndLastName()
    {
        // Given
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName("Alexanderson")
            .WithLastName("Constantinopolus")
            .Build();

        // When
        var identifierName = _subject.GenerateIdentifierName(profile);
        var (namePart, suffix) = ExtractIdentifierParts(identifierName);

        // Then
        identifierName.Length.Should().BeLessThanOrEqualTo(20);
        namePart.Should().HaveLength(20 - 6 - 1); // 20 total - 6 for suffix - 1 for dash
        namePart.Should().Be("constantinopo");
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
        namePart.Should().NotContain("--").And.NotStartWith("-").And.NotEndWith("-");
    }

    [Theory]
    [InlineData("Maria", null, "maria")]
    [InlineData("Maria", "", "maria")]
    [InlineData("Maria", "  ", "maria")]
    public void GenerateCorrectIdentifierGivenOnlyFirstName(string firstName, string? lastName, string expectedNamePart)
    {
        // Given
        var profile = new ProfileEntityBuilder(_fixture)
            .Build();
        profile.FirstName = firstName;
        profile.LastName = lastName;

        // When
        var identifierName = _subject.GenerateIdentifierName(profile);
        var (namePart, suffix) = ExtractIdentifierParts(identifierName);

        // Then
        identifierName.Length.Should().BeLessThanOrEqualTo(20);
        namePart.Should().Be(expectedNamePart);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    [Theory]
    [InlineData(null, "Fernández", "fernandez")]
    [InlineData("", "Fernández", "fernandez")]
    [InlineData("  ", "Fernández", "fernandez")]
    public void GenerateCorrectIdentifierGivenOnlyLastName(string? firstName, string lastName, string expectedNamePart)
    {
        // Given
        var profile = new ProfileEntityBuilder(_fixture)
            .Build();

        profile.FirstName = firstName;
        profile.LastName = lastName;

        // When
        var identifierName = _subject.GenerateIdentifierName(profile);
        var (namePart, suffix) = ExtractIdentifierParts(identifierName);

        // Then
        identifierName.Length.Should().BeLessThanOrEqualTo(20);
        namePart.Should().Be(expectedNamePart);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    [Theory]
    [InlineData(null, null, "profile")]
    [InlineData("", "", "profile")]
    [InlineData("  ", "  ", "profile")]
    public void GenerateCorrectIdentifierGivenDefaultNamePartGivenNullOrEmptyNames(string? firstName, string? lastName, string expectedNamePart)
    {
        // Given
        var profile = new ProfileEntityBuilder(_fixture)
            .Build();
        profile.FirstName = firstName;
        profile.LastName = lastName;

        // When
        var identifierName = _subject.GenerateIdentifierName(profile);
        var (namePart, suffix) = ExtractIdentifierParts(identifierName);

        // Then
        identifierName.Length.Should().BeLessThanOrEqualTo(20);
        namePart.Should().Be(expectedNamePart);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    [Fact]
    public void GenerateIdentifierGivenNameContainingMultipleSpacesAndUnderscores()
    {
        // Given
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName("Ana_Sofia")
            .WithLastName("De La   Vega")
            .Build();

        // When
        var identifierName = _subject.GenerateIdentifierName(profile);
        var (namePart, suffix) = ExtractIdentifierParts(identifierName);

        // Then
        identifierName.Length.Should().BeLessThanOrEqualTo(20);
        namePart.Should().Be("ana-vega");
        "ana-vega".Should().StartWith(namePart);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    [Fact]
    public void GenerateDefaultIdentifierGivenNameConsistingOnlyOfSpecialChars()
    {
        // Given
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName("!@#$%")
            .WithLastName("^&*()")
            .Build();

        // When
        var identifierName = _subject.GenerateIdentifierName(profile);
        var (namePart, suffix) = ExtractIdentifierParts(identifierName);

        // Then
        identifierName.Length.Should().BeLessThanOrEqualTo(20);
        namePart.Should().Be("profile");
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
        identifierName.Should().StartWith("profile-");
    }

    private static (string NamePart, string Suffix) ExtractIdentifierParts(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            return (string.Empty, string.Empty);
        }

        var match = RemoveSpecialCharacters().Match(identifier);
        if (match is { Success: true, Groups.Count: 3 })
        {
            return (match.Groups[1].Value, match.Groups[2].Value);
        }

        // Casos especiales (ej. "id-sufijo" o solo sufijo si el nombre se vacía o es muy corto)
        // Si el identificador es "id-xxxxxx"
        if (identifier.StartsWith("id-") && identifier.Length == "id-".Length + 6)
        {
            return ("id", identifier.Substring(3));
        }
        // Si el identificador es solo el sufijo (porque maxNamePartLength < 1)
        if (identifier.Length <= 6 && Regex.IsMatch(identifier, @"^[a-z0-9]+$"))
        {
            return (string.Empty, identifier);
        }

        // Fallback si no se puede parsear claramente, aunque los tests deberían cubrir patrones esperados
        // Para los tests, es mejor ser específico. Si un test espera un formato que no encaja aquí,
        // podría necesitar una aserción directa sobre el string completo.
        var lastDash = identifier.LastIndexOf('-');
        if (lastDash > 0 && identifier.Length - 1 - lastDash == 6) // heurística
            return (identifier.Substring(0, lastDash), identifier.Substring(lastDash + 1));

        return (identifier, string.Empty); // No se pudo determinar un sufijo claro de 6 dígitos
    }

    [GeneratedRegex(@"^(.*)-([a-z0-9]{6})$")]
    private static partial Regex RemoveSpecialCharacters();
}