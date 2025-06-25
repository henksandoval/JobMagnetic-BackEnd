using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SummaryCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SummaryEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.ProfileId, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );

        fixture.Register(() =>
            new SummaryRaw(
                FixtureBuilder.Faker.Lorem.Paragraph(),
                [],
                []
            )
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Profile = FixtureBuilder.Build().Create<ProfileEntity>();
        item.Introduction = FixtureBuilder.Faker.Lorem.Paragraph();
        item.WorkExperiences = FixtureBuilder.Build().CreateMany<WorkExperienceEntity>().ToList();
    }
}