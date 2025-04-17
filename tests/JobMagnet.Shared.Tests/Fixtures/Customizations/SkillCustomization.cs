using AutoFixture;
using Bogus;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SkillCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var faker = new Faker();

        fixture.Customize<SkillEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .With(x => x.Overview, faker.Lorem.Sentence())
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
                .With(x => x.SkillDetails, fixture.CreateMany<SkillItemEntity>().ToList())
                .OmitAutoProperties()
        );
    }
}