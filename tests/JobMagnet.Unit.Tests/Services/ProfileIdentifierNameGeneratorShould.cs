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

    [Theory]
    [InlineData("Juan", "Aceituno", "juan-aceituno")]
    [InlineData("Ernesto", "Sosa", "ernesto-sosa")]
    [InlineData("Ana", "Armas", "ana-armas")]
    public void GenerateCorrectIdentifierGivenNormalFirstAndLastName(string firstName, string? lastName, string expectedNamePart)
    {
        // Given
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName(firstName)
            .WithLastName(lastName)
            .Build();

        // When
        var identifierName = _subject.GenerateIdentifierName(profile);
        var (namePart, suffix) = ExtractIdentifierParts(identifierName);

        // Then
        identifierName.Length.Should().BeLessThanOrEqualTo(20);
        namePart.Should().Be(expectedNamePart);
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

    [Theory]
    [InlineData("Ana-Sofia", "Gutierrez_Vega", "ana-gutierrez")]
    [InlineData("Ana-Sofia", "De La   Vega", "ana-vega")]
    [InlineData("ana-sofia", "de-la-vega", "ana-vega")]
    public void GenerateIdentifierGivenNameContainingMultipleSpacesAndUnderscores(string firstName, string lastName, string expectedNamePart)
    {
        // Given
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName(firstName)
            .WithLastName(lastName)
            .Build();

        // When
        var identifierName = _subject.GenerateIdentifierName(profile);
        var (namePart, suffix) = ExtractIdentifierParts(identifierName);

        // Then
        identifierName.Length.Should().BeLessThanOrEqualTo(20);
        namePart.Should().Be(expectedNamePart);
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

        var match = PatternWithSixCharAlphanumericSuffixRegex().Match(identifier);
        if (match is { Success: true, Groups.Count: 3 })
        {
            return (match.Groups[1].Value, match.Groups[2].Value);
        }

        // Si el identificador es solo el sufijo (porque maxNamePartLength < 1)
        if (identifier.Length <= 6 && LowercaseAlphanumericStringRegex().IsMatch(identifier))
        {
            return (string.Empty, identifier);
        }

        // Fallback si no se puede parsear claramente, aunque los tests deberían cubrir patrones esperados
        // Para los tests, es mejor ser específico. Si un test espera un formato que no encaja aquí,
        // podría necesitar una aserción directa sobre el string completo.
        var lastDash = identifier.LastIndexOf('-');
        if (lastDash > 0 && identifier.Length - 1 - lastDash == 6) // heurística
            return (identifier[..lastDash], identifier[(lastDash + 1)..]);

        return (identifier, string.Empty); // No se pudo determinar un sufijo claro de 6 dígitos
    }

    [GeneratedRegex("^(.*)-([a-z0-9]{6})$")]
    private static partial Regex PatternWithSixCharAlphanumericSuffixRegex();

    [GeneratedRegex("^[a-z0-9]+$")]
    private static partial Regex LowercaseAlphanumericStringRegex();
}