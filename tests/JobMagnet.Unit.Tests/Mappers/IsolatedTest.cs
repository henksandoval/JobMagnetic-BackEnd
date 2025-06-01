using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class IsolatedTest
{
    [Fact]
    public void Test()
    {
        var fixture = FixtureBuilder.Build();
        var resume = fixture.Create<IList<ResumeRaw>>();
    }
}