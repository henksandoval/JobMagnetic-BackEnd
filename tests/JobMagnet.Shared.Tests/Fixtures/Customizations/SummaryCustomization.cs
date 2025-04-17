using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SummaryCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SummaryEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Do(ApplyCommonProperties)
                .Without(x => x.Education)
                .Without(x => x.WorkExperiences)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Introduction = FixtureBuilder.Faker.Lorem.Paragraph();
    }
}