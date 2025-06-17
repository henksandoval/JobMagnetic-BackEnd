using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Services;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Application.UseCases.CvParser.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using Moq;

namespace JobMagnet.Unit.Tests.Services;

public class ProfileFactoryServiceShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly ProfileRawBuilder _profileBuilder;
    private readonly ProfileFactoryService _profileFactoryService;
    private readonly Mock<IQueryRepository<SkillType, int>> _skillTypeRepository;
    private readonly Mock<IQueryRepository<ContactTypeEntity, int>> _contactTypeRepository;

    public ProfileFactoryServiceShould()
    {
        _profileBuilder = new ProfileRawBuilder(_fixture);
        _skillTypeRepository = new Mock<IQueryRepository<SkillType, int>>();
        _contactTypeRepository = new Mock<IQueryRepository<ContactTypeEntity, int>>();
        _profileFactoryService = new ProfileFactoryService(
            _skillTypeRepository.Object,
            _contactTypeRepository.Object);
    }

    [Fact(DisplayName = "Generate correctly profile when have ProfileDto with only first level properties")]
    public async Task GenerateCorrectlyProfileWhenHaveProfileProperties()
    {
        var profileDto = _profileBuilder
            .Build()
            .ToProfileParseDto();

        var profile = await _profileFactoryService.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        profile.Should().NotBeNull();
        profile.Should().BeEquivalentTo(profileDto, options => options
            .WithMapping<ProfileEntity>(entity => entity.FirstName, dto => dto.FirstName)
            .WithMapping<ProfileEntity>(entity => entity.SecondLastName, dto => dto.SecondLastName)
            .WithMapping<ProfileEntity>(entity => entity.LastName, dto => dto.LastName)
            .WithMapping<ProfileEntity>(entity => entity.SecondLastName, dto => dto.SecondLastName)
            .WithMapping<ProfileEntity>(entity => entity.BirthDate, dto => dto.BirthDate)
            .ExcludingMissingMembers()
        );
    }

    [Fact(DisplayName = "Generate correctly profile when have ProfileDto with only first level properties")]
    public async Task GenerateCorrectlyProfileWhenHaveTalents()
    {
        var profileDto = _profileBuilder
            .WithTalents()
            .Build()
            .ToProfileParseDto();

        var profile = await _profileFactoryService.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        profile.Should().NotBeNull();
    }
}