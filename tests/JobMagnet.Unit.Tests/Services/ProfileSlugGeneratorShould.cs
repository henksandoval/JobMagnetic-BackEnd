using System.Text.RegularExpressions;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Services;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;

namespace JobMagnet.Unit.Tests.Services;

public partial class ProfileSlugGeneratorShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly ProfileSlugGenerator _subject = new ();

    [GeneratedRegex("^(.*)-([a-z0-9]{6})$")]
    private static partial Regex PatternWithSixCharAlphanumericSuffixRegex();

    [GeneratedRegex("^[a-z0-9]+$")]
    private static partial Regex LowercaseAlphanumericStringRegex();

    [Theory]
    [InlineData("Juan", "Aceituno", "juan-aceituno")]
    [InlineData("Ernesto", "Sosa", "ernesto-sosa")]
    [InlineData("Ana", "Armas", "ana-armas")]
    public void GenerateCorrectSlugGivenNormalFirstAndLastName(string firstName, string? lastName, string expectedNamePart)
    {
        // --- Given ---
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName(firstName)
            .WithLastName(lastName)
            .Build();

        // --- When ---
        var slug = _subject.GenerateProfileSlug(profile);
        var (namePart, suffix) = ExtractSlugParts(slug);

        // --- Then ---
        AssertValidSlug(slug);
        namePart.Should().Be(expectedNamePart);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
        namePart.Should().NotContain("--").And.NotStartWith("-").And.NotEndWith("-");
    }

    [Fact]
    public void GenerateCorrectSlugGivenNameContainingSpecialCharsAndAccents()
    {
        // --- Given ---
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName("María")
            .WithLastName("Ñíguez")
            .Build();

        // --- When ---
        var slug = _subject.GenerateProfileSlug(profile);
        var (namePart, suffix) = ExtractSlugParts(slug);

        // --- Then ---
        AssertValidSlug(slug);
        namePart.Should().Be("maria-niguez");
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    [Fact]
    public void GenerateCorrectSlugGivenNameContainingSpecialCharsAccentsAndSpaces()
    {
        // --- Given ---
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName("José María")
            .WithLastName("López-Ñíguez")
            .Build();

        // --- When ---
        var slug = _subject.GenerateProfileSlug(profile);
        var (namePart, suffix) = ExtractSlugParts(slug);

        // --- Then ---
        AssertValidSlug(slug);
        namePart.Should().Be("jose-lopez");
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    [Theory]
    [InlineData("Maria", null, "maria")]
    [InlineData("Maria", "", "maria")]
    [InlineData("Maria", "  ", "maria")]
    public void GenerateCorrectSlugGivenOnlyFirstName(string firstName, string? lastName, string expectedNamePart)
    {
        // --- Given ---
        var profile = new ProfileEntityBuilder(_fixture)
            .Build();
        profile.FirstName = firstName;
        profile.LastName = lastName;

        // --- When ---
        var slug = _subject.GenerateProfileSlug(profile);
        var (namePart, suffix) = ExtractSlugParts(slug);

        // --- Then ---
        AssertValidSlug(slug);
        namePart.Should().Be(expectedNamePart);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    [Theory]
    [InlineData(null, "Fernández", "fernandez")]
    [InlineData("", "Fernández", "fernandez")]
    [InlineData("  ", "Fernández", "fernandez")]
    public void GenerateCorrectSlugGivenOnlyLastName(string? firstName, string lastName, string expectedNamePart)
    {
        // --- Given ---
        var profile = new ProfileEntityBuilder(_fixture)
            .Build();

        profile.FirstName = firstName;
        profile.LastName = lastName;

        // --- When ---
        var slug = _subject.GenerateProfileSlug(profile);
        var (namePart, suffix) = ExtractSlugParts(slug);

        // --- Then ---
        AssertValidSlug(slug);
        namePart.Should().Be(expectedNamePart);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    [Theory]
    [InlineData(null, null, PublicProfileIdentifierEntity.DefaultSlug)]
    [InlineData("", "", PublicProfileIdentifierEntity.DefaultSlug)]
    [InlineData("  ", "  ", PublicProfileIdentifierEntity.DefaultSlug)]
    public void GenerateCorrectSlugGivenDefaultNamePartGivenNullOrEmptyNames(string? firstName, string? lastName, string expectedNamePart)
    {
        // --- Given ---
        var profile = new ProfileEntityBuilder(_fixture)
            .Build();
        profile.FirstName = firstName;
        profile.LastName = lastName;

        // --- When ---
        var slug = _subject.GenerateProfileSlug(profile);
        var (namePart, suffix) = ExtractSlugParts(slug);

        // --- Then ---
        AssertValidSlug(slug);
        namePart.Should().Be(expectedNamePart);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    [Theory]
    [InlineData("Ana-Sofia", "Gutierrez_Vega", "ana-gutierrez")]
    [InlineData("Ana-Sofia", "De La   Vega", "ana-vega")]
    [InlineData("ana-sofia", "de-la-vega", "ana-vega")]
    public void GenerateSlugGivenNameContainingMultipleSpacesAndUnderscores(string firstName, string lastName, string expectedNamePart)
    {
        // --- Given ---
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName(firstName)
            .WithLastName(lastName)
            .Build();

        // --- When ---
        var slug = _subject.GenerateProfileSlug(profile);
        var (namePart, suffix) = ExtractSlugParts(slug);

        // --- Then ---
        AssertValidSlug(slug);
        namePart.Should().Be(expectedNamePart);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    [Fact]
    public void GenerateDefaultSlugGivenNameConsistingOnlyOfSpecialChars()
    {
        // --- Given ---
        var profile = new ProfileEntityBuilder(_fixture)
            .WithName("!@#$%")
            .WithLastName("^&*()")
            .Build();

        // --- When ---
        var slug = _subject.GenerateProfileSlug(profile);
        var (namePart, suffix) = ExtractSlugParts(slug);

        // --- Then ---
        AssertValidSlug(slug);
        namePart.Should().Be(PublicProfileIdentifierEntity.DefaultSlug);
        suffix.Should().HaveLength(6).And.MatchRegex("^[a-z0-9]{6}$");
    }

    private static void AssertValidSlug(string slug)
    {
        slug.Should().NotBeNullOrEmpty();
        slug.Length.Should().BeLessThanOrEqualTo(PublicProfileIdentifierEntity.MaxNameLength);
        slug.Should().MatchRegex("^[a-z0-9-]+$");
        slug.Should().NotContain("--").And.NotStartWith("-").And.NotEndWith("-");
    }

    private static (string NamePart, string Suffix) ExtractSlugParts(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return (string.Empty, string.Empty);
        }

        var match = PatternWithSixCharAlphanumericSuffixRegex().Match(slug);
        if (match is { Success: true, Groups.Count: 3 })
        {
            return (match.Groups[1].Value, match.Groups[2].Value);
        }

        if (slug.Length <= 6 && LowercaseAlphanumericStringRegex().IsMatch(slug))
        {
            return (string.Empty, slug);
        }

        var lastDash = slug.LastIndexOf('-');
        if (lastDash > 0 && slug.Length - 1 - lastDash == 6)
            return (slug[..lastDash], slug[(lastDash + 1)..]);

        return (slug, string.Empty);
    }
}