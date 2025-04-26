using AutoFixture;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class WorkExperienceCustomization : ICustomization
{
    private static readonly string[] JobTitle =
    [
        "Software Engineer",
        "Data Scientist",
        "Project Manager",
        "Marketing Specialist",
        "Graphic Designer",
        "Financial Analyst",
        "DevOps Engineer",
        "Product Manager",
        "Cybersecurity Analyst",
        "Cloud Architect"
    ];

    private static readonly string[] CompanyName =
    [
        "Google",
        "Microsoft",
        "Amazon",
        "Apple",
        "Facebook (Meta)",
        "Tesla",
        "Netflix",
        "Adobe",
        "IBM",
        "Intel"
    ];

    private static readonly string[] Description =
    [
        "Developed and maintained software applications using modern technologies.",
        "Analyzed complex datasets to extract actionable insights for strategic decision-making.",
        "Managed cross-functional teams to deliver projects on time and within budget.",
        "Designed and implemented marketing campaigns to increase brand awareness.",
        "Created visually appealing designs for digital and print media.",
        "Conducted financial analysis to support investment decisions and business strategies.",
        "Built and maintained CI/CD pipelines for seamless software deployment.",
        "Led product development efforts, aligning teams with business goals.",
        "Monitored and mitigated security threats to ensure data integrity.",
        "Designed and implemented scalable cloud-based solutions for enterprise clients."
    ];

    public void Customize(IFixture fixture)
    {
        fixture.Customize<WorkExperienceEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.JobTitle = FixtureBuilder.Faker.PickRandom(JobTitle);
        item.CompanyName = FixtureBuilder.Faker.PickRandom(CompanyName);
        item.CompanyLocation = FixtureBuilder.Faker.Address.FullAddress();
        item.StartDate = FixtureBuilder.Faker.Date.Past(20, DateTime.Now.AddYears(-5));
        item.EndDate =
            TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)));
        item.Description = FixtureBuilder.Faker.PickRandom(Description);
    }
}