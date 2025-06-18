using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Factories;
using JobMagnet.Application.Services;
using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Application.UseCases.CvParser.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using Moq;

namespace JobMagnet.Unit.Tests.Services;

public class ProfileFactoryShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly ProfileRawBuilder _profileBuilder;
    private readonly ProfileFactory _profileFactory;
    private readonly Mock<IQueryRepository<SkillType, int>> _skillTypeRepository;
    private readonly Mock<IQueryRepository<ContactTypeEntity, int>> _contactTypeRepository;

    public ProfileFactoryShould()
    {
        _profileBuilder = new ProfileRawBuilder(_fixture);
        _skillTypeRepository = new Mock<IQueryRepository<SkillType, int>>();
        _contactTypeRepository = new Mock<IQueryRepository<ContactTypeEntity, int>>();
        _profileFactory = new ProfileFactory(
            _skillTypeRepository.Object,
            _contactTypeRepository.Object);
    }

    [Fact(DisplayName = "Map root properties from a simple DTO")]
    public async Task MapRootProperties_FromSimpleDto()
    {
        var profileDto = _profileBuilder
            .Build()
            .ToProfileParseDto();

        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        profile.Should().NotBeNull();
        profile.Should().BeEquivalentTo(profileDto, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map talents collection when the DTO provides them")]
    public async Task MapTalents_WhenDtoProvidesThem()
    {
        var profileDto = _profileBuilder
            .WithTalents()
            .Build()
            .ToProfileParseDto();

        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        profile.Should().NotBeNull();
        profile.Talents.Should().BeEquivalentTo(profileDto.Talents, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map testimonials collection when the DTO provides them")]
    public async Task MapTestimonials_WhenDtoProvidesThem()
    {
        var profileDto = _profileBuilder
            .WithTestimonials()
            .Build()
            .ToProfileParseDto();

        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        profile.Should().NotBeNull();
        profile.Testimonials.Should().BeEquivalentTo(profileDto.Testimonials, options => options.ExcludingMissingMembers());
    }
}