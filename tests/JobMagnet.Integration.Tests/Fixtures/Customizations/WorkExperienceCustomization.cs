using AutoFixture;
using Bogus;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Integration.Tests.Utils;

namespace JobMagnet.Integration.Tests.Fixtures.Customizations;

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

    private readonly Faker _faker = new();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<WorkExperienceEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private void ApplyCommonProperties(dynamic item)
    {
        item.JobTitle = _faker.PickRandom(JobTitle);
        item.CompanyName = _faker.PickRandom(CompanyName);
        item.CompanyLocation = _faker.Address.FullAddress();
        item.StartDate = _faker.Date.Past(20, DateTime.Now.AddYears(-5));
        item.EndDate = TestUtilities.OptionalValue(_faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)));
        item.Description = _faker.PickRandom(Description);
    }
}