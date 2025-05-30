namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public static class StaticCustomizations
{
    public static readonly List<(string Name, string IconClass)> ContactTypes = new()
    {
        ("Email", "bx bx-envelope"),
        ("Mobile Phone", "bx bx-mobile"),
        ("Home Phone", "bx bx-phone"),
        ("Work Phone", "bx bx-phone-call"),
        ("Website", "bx bx-globe"),
        ("LinkedIn", "bx bxl-linkedin"),
        ("GitHub", "bx bxl-github"),
        ("Twitter", "bx bxl-twitter"),
        ("Facebook", "bx bxl-facebook"),
        ("Instagram", "bx bxl-instagram"),
        ("YouTube", "bx bxl-youtube"),
        ("WhatsApp", "bx bxl-whatsapp"),
        ("Telegram", "bx bxl-telegram"),
        ("Snapchat", "bx bxl-snapchat"),
        ("Pinterest", "bx bxl-pinterest"),
        ("Skype", "bx bxl-skype"),
        ("Discord", "bx bxl-discord"),
        ("Twitch", "bx bxl-twitch"),
        ("TikTok", "bx bxl-tiktok"),
        ("Reddit", "bx bxl-reddit"),
        ("Vimeo", "bx bxl-vimeo")
    };

    public static readonly string[] Degrees =
    [
        "Bachelor's in Computer Science",
        "Master's in Business Administration",
        "PhD in Physics",
        "Associate Degree in Psychology",
        "Bachelor's in Mechanical Engineering",
        "Diploma in Graphic Design",
        "MBA in Marketing"
    ];

    public static readonly string[] Universities =
    [
        "Harvard University",
        "Stanford University",
        "Massachusetts Institute of Technology",
        "University of Cambridge",
        "University of Oxford",
        "California Institute of Technology",
        "University of Tokyo",
        "National University of Singapore",
        "University of Toronto",
        "University of Melbourne"
    ];

    public static readonly string[] Talents =
    [
        "Creative",
        "Problem Solver",
        "Team Player",
        "Adaptable",
        "Detail-Oriented",
        "Strong Communicator",
        "Analytical"
    ];

    public static readonly string[] JobTitles =
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

    public static readonly string[] CompanyNames =
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

    public static readonly string[] Descriptions =
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
}