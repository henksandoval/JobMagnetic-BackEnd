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
                .With(x => x.Introduction, FixtureBuilder.Faker.Lorem.Paragraph())
                .With(x => x.IsDeleted, false)
                .Without(x => x.Education)
                .Without(x => x.WorkExperiences)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
        );
    }
}