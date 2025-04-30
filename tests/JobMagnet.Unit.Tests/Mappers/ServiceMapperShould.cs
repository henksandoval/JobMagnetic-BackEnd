using AutoFixture;
using FluentAssertions;
using JobMagnet.Mappers;
using JobMagnet.Models.Commands.Service;
using JobMagnet.Shared.Tests.Fixtures;

namespace JobMagnet.Unit.Tests.Mappers;

public class ServiceMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact]
    public void MapServiceCreateCommandToServiceEntityCorrectly()
    {
        // Given
        var createCommand = _fixture.Create<ServiceCreateCommand>();

        // When
        var entity = ServiceMapper.ToEntity(createCommand);

        // Then
        entity.Should().NotBeNull();
        entity.Should().BeEquivalentTo(createCommand.ServiceData);
    }
}