using System.Text.RegularExpressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Services;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;

namespace JobMagnet.Unit.Tests.Services;

public partial class ProfileIdentifierNameGeneratorShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly ProfileIdentifierNameGenerator _subject = new ();

    [GeneratedRegex("^(.*)-([a-z0-9]{6})$")]
    private static partial Regex PatternWithSixCharAlphanumericSuffixRegex();

    [GeneratedRegex("^[a-z0-9]+$")]
    private static partial Regex LowercaseAlphanumericStringRegex();

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
        AssertValidIdentifier(identifierName);
        namePart.Should().Be(expectedNamePart);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
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
        AssertValidIdentifier(identifierName);
        namePart.Should().Be("maria-niguez");
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
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
        AssertValidIdentifier(identifierName);
        namePart.Should().Be("jose-lopez");
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
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
        AssertValidIdentifier(identifierName);
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
        AssertValidIdentifier(identifierName);
        namePart.Should().Be(expectedNamePart);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    [Theory]
    [InlineData(null, null, PublicProfileIdentifierEntity.DefaultIdentifierName)]
    [InlineData("", "", PublicProfileIdentifierEntity.DefaultIdentifierName)]
    [InlineData("  ", "  ", PublicProfileIdentifierEntity.DefaultIdentifierName)]
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
        AssertValidIdentifier(identifierName);
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
        AssertValidIdentifier(identifierName);
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
        AssertValidIdentifier(identifierName);
        namePart.Should().Be(PublicProfileIdentifierEntity.DefaultIdentifierName);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    private static void AssertValidIdentifier(string identifier)
    {
        identifier.Should().NotBeNullOrEmpty();
        identifier.Length.Should().BeLessThanOrEqualTo(PublicProfileIdentifierEntity.MaxNameLength);
        identifier.Should().MatchRegex("^[a-z0-9-]+$");
        identifier.Should().NotContain("--").And.NotStartWith("-").And.NotEndWith("-");
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

        if (identifier.Length <= 6 && LowercaseAlphanumericStringRegex().IsMatch(identifier))
        {
            return (string.Empty, identifier);
        }

        var lastDash = identifier.LastIndexOf('-');
        if (lastDash > 0 && identifier.Length - 1 - lastDash == 6)
            return (identifier[..lastDash], identifier[(lastDash + 1)..]);

        return (identifier, string.Empty);
    }
}