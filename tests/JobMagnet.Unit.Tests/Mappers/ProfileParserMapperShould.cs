using AutoFixture;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;

namespace JobMagnet.Unit.Tests.Mappers;

public class ProfileParserMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact(DisplayName = "Map ProfileParseDto to ProfileCommand when all properties are defined")]
    public void MapperProfileParseDtoToProfileCommand()
    {
        var profileBuilder = new ProfileParseDtoBuilder(_fixture)
            .WithResume()
            .WithSkills()
            .WithServices()
            .WithContactInfo()
            .WithTalents()
            .WithPortfolio()
            .WithSummaries()
            .WithTestimonials();

        var profile = profileBuilder.Build();


    }
}