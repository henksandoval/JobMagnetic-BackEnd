namespace JobMagnet.Shared.Data;

public record RawSkillDefinition(int SkillId, string Name, string Uri, RawSimpleDefinition Category, List<RawSimpleDefinition> Aliases);

public static class SkillRawData
{
    private const string DefaultIconUri = "https://jobmagnet.com/default-icon.png";
    private static readonly RawSimpleDefinition CatSoftwareDev = new(2, "Software Development");
    private static readonly RawSimpleDefinition CatDatabases = new(3, "Bases de Datos");
    private static readonly RawSimpleDefinition CatCloudDevOps = new(4, "Cloud y DevOps");
    private static readonly RawSimpleDefinition CatArchitecture = new(5, "Arquitectura y Patrones");
    private static readonly RawSimpleDefinition CatTesting = new(6, "Testing");
    private static readonly RawSimpleDefinition CatDataScience = new(7, "Data Science y ML");
    private static readonly RawSimpleDefinition CatMethodologies = new(8, "Metodologías y Gestión");
    private static readonly RawSimpleDefinition CatSoftSkills = new(9, "Habilidades Blandas");

    public static readonly IReadOnlyList<RawSkillDefinition> Data =
    [
        new(101, "HTML", "https://cdn.simpleicons.org/html5", CatSoftwareDev,
        [
            new RawSimpleDefinition(206, "HTML5")
        ]),
        new(102, "CSS", "https://cdn.simpleicons.org/css3", CatSoftwareDev,
        [
            new RawSimpleDefinition(207, "CSS3")
        ]),
        new(103, "JavaScript", "https://cdn.simpleicons.org/javascript", CatSoftwareDev,
        [
            new RawSimpleDefinition(201, "JS")
        ]),
        new(104, "C#", "https://cdn.simpleicons.org/csharp", CatSoftwareDev, []),
        new(105, "TypeScript", "https://cdn.simpleicons.org/typescript", CatSoftwareDev,
        [
            new RawSimpleDefinition(202, "TS")
        ]),
        new(106, "Angular", "https://cdn.simpleicons.org/angular", CatSoftwareDev, []),
        new(107, "PostgreSQL", "https://cdn.simpleicons.org/postgresql", CatDatabases,
        [
            new RawSimpleDefinition(203, "Postgres")
        ]),
        new(108, "React", "https://cdn.simpleicons.org/react", CatSoftwareDev, []),
        new(109, "Bootstrap", "https://cdn.simpleicons.org/bootstrap", CatSoftwareDev, []),
        new(110, "Vue", "https://cdn.simpleicons.org/vuedotjs", CatSoftwareDev,
        [
            new RawSimpleDefinition(204, "Vue.js")
        ]),
        new(111, "Git", "https://cdn.simpleicons.org/git", CatCloudDevOps, []),
        new(112, "Blazor", "https://cdn.simpleicons.org/blazor", CatSoftwareDev, []),
        new(113, "RabbitMQ", "https://cdn.simpleicons.org/rabbitmq", CatSoftwareDev,
        [
            new RawSimpleDefinition(205, "Rabbit MQ")
        ]),
        new(114, "Docker", "https://cdn.simpleicons.org/docker", CatCloudDevOps, []),
        new(115, "Java", "https://cdn.simpleicons.org/java", CatSoftwareDev, []),
        new(116, "Python", "https://cdn.simpleicons.org/python", CatSoftwareDev, []),
        new(117, "Go", "https://cdn.simpleicons.org/go", CatSoftwareDev, []),
        new(118, "Rust", "https://cdn.simpleicons.org/rust", CatSoftwareDev, []),
        new(119, "Kotlin", "https://cdn.simpleicons.org/kotlin", CatSoftwareDev, []),
        new(120, "Swift", "https://cdn.simpleicons.org/swift", CatSoftwareDev, []),
        new(121, "PHP", "https://cdn.simpleicons.org/php", CatSoftwareDev, []),
        new(122, "Ruby", "https://cdn.simpleicons.org/ruby", CatSoftwareDev, []),
        new(123, "C++", "https://cdn.simpleicons.org/cplusplus", CatSoftwareDev, []),
        new(124, "SQL", "https://cdn.simpleicons.org/azuredatastudio", CatDatabases, []),
        new(125, ".NET", "https://cdn.simpleicons.org/dotnet", CatSoftwareDev, []),
        new(126, "ASP.NET Core", "https://cdn.simpleicons.org/dotnet", CatSoftwareDev,
        [
            new RawSimpleDefinition(208, "ASP.NET")
        ]),
        new(127, "Spring Boot", "https://cdn.simpleicons.org/springboot", CatSoftwareDev,
        [
            new RawSimpleDefinition(209, "Spring")
        ]),
        new(128, "Django", "https://cdn.simpleicons.org/django", CatSoftwareDev, []),
        new(129, "Flask", "https://cdn.simpleicons.org/flask", CatSoftwareDev, []),
        new(130, "Node.js", "https://cdn.simpleicons.org/nodedotjs", CatSoftwareDev,
        [
            new RawSimpleDefinition(210, "Node")
        ]),
        new(131, "Express.js", "https://cdn.simpleicons.org/express", CatSoftwareDev,
        [
            new RawSimpleDefinition(211, "Express")
        ]),
        new(132, "Ruby on Rails", "https://cdn.simpleicons.org/rubyonrails", CatSoftwareDev,
        [
            new RawSimpleDefinition(212, "Rails")
        ]),
        new(133, "Laravel", "https://cdn.simpleicons.org/laravel", CatSoftwareDev, []),
        new(134, "Entity Framework Core", "https://cdn.simpleicons.org/dotnet", CatSoftwareDev,
        [
            new RawSimpleDefinition(213, "EF Core")
        ]),
        new(135, "Svelte", "https://cdn.simpleicons.org/svelte", CatSoftwareDev, []),
        new(136, "jQuery", "https://cdn.simpleicons.org/jquery", CatSoftwareDev, []),
        new(137, "Sass", "https://cdn.simpleicons.org/sass", CatSoftwareDev,
        [
            new RawSimpleDefinition(214, "SCSS")
        ]),
        new(138, "Tailwind CSS", "https://cdn.simpleicons.org/tailwindcss", CatSoftwareDev, []),
        new(139, "SQL Server", "https://cdn.simpleicons.org/microsoftsqlserver", CatDatabases, []),
        new(140, "MySQL", "https://cdn.simpleicons.org/mysql", CatDatabases, []),
        new(141, "Oracle Database", "https://cdn.simpleicons.org/oracle", CatDatabases,
        [
            new RawSimpleDefinition(215, "Oracle")
        ]),
        new(142, "MongoDB", "https://cdn.simpleicons.org/mongodb", CatDatabases, []),
        new(143, "Redis", "https://cdn.simpleicons.org/redis", CatDatabases, []),
        new(144, "Cassandra", "https://cdn.simpleicons.org/apachecassandra", CatDatabases, []),
        new(145, "SQLite", "https://cdn.simpleicons.org/sqlite", CatDatabases, []),
        new(146, "Cosmos DB", "https://cdn.simpleicons.org/azurecosmosdb", CatDatabases, []),
        new(147, "DynamoDB", "https://cdn.simpleicons.org/amazondynamodb", CatDatabases, []),
        new(148, "Microsoft Azure", "https://cdn.simpleicons.org/microsoftazure", CatCloudDevOps,
        [
            new RawSimpleDefinition(216, "Azure")
        ]),
        new(149, "Amazon Web Services", "https://cdn.simpleicons.org/amazonaws", CatCloudDevOps,
        [
            new RawSimpleDefinition(217, "AWS")
        ]),
        new(150, "Google Cloud Platform", "https://cdn.simpleicons.org/googlecloud", CatCloudDevOps,
        [
            new RawSimpleDefinition(218, "GCP")
        ]),
        new(151, "Kubernetes", "https://cdn.simpleicons.org/kubernetes", CatCloudDevOps,
        [
            new RawSimpleDefinition(219, "k8s")
        ]),
        new(152, "Terraform", "https://cdn.simpleicons.org/terraform", CatCloudDevOps, []),
        new(153, "Ansible", "https://cdn.simpleicons.org/ansible", CatCloudDevOps, []),
        new(154, "Jenkins", "https://cdn.simpleicons.org/jenkins", CatCloudDevOps, []),
        new(155, "Azure DevOps", "https://cdn.simpleicons.org/azuredevops", CatCloudDevOps, []),
        new(156, "GitHub Actions", "https://cdn.simpleicons.org/githubactions", CatCloudDevOps, []),
        new(157, "GitLab CI", "https://cdn.simpleicons.org/gitlab", CatCloudDevOps,
        [
            new RawSimpleDefinition(220, "GitLab")
        ]),
        new(158, "Prometheus", "https://cdn.simpleicons.org/prometheus", CatCloudDevOps, []),
        new(159, "Grafana", "https://cdn.simpleicons.org/grafana", CatCloudDevOps, []),
        new(160, "Microservices Architecture", DefaultIconUri, CatArchitecture,
        [
            new RawSimpleDefinition(221, "Microservicios")
        ]),
        new(161, "Domain-Driven Design", DefaultIconUri, CatArchitecture,
        [
            new RawSimpleDefinition(222, "DDD")
        ]),
        new(162, "SOLID Principles", DefaultIconUri, CatArchitecture,
        [
            new RawSimpleDefinition(223, "SOLID")
        ]),
        new(163, "REST APIs", DefaultIconUri, CatArchitecture,
        [
            new RawSimpleDefinition(224, "REST")
        ]),
        new(164, "GraphQL", "https://cdn.simpleicons.org/graphql", CatArchitecture, []),
        new(165, "Event-Driven Architecture", DefaultIconUri, CatArchitecture, []),
        new(166, "CQRS", DefaultIconUri, CatArchitecture, []),
        new(167, "Design Patterns", DefaultIconUri, CatArchitecture, []),
        new(168, "Unit Testing", DefaultIconUri, CatTesting, []),
        new(169, "Integration Testing", DefaultIconUri, CatTesting, []),
        new(170, "End-to-End Testing", DefaultIconUri, CatTesting,
        [
            new RawSimpleDefinition(225, "E2E Testing")
        ]),
        new(171, "xUnit", "https://cdn.simpleicons.org/xunit", CatTesting, []),
        new(172, "NUnit", "https://cdn.simpleicons.org/nunit", CatTesting, []),
        new(173, "MSTest", "https://cdn.simpleicons.org/visualstudio", CatTesting, []),
        new(174, "Selenium", "https://cdn.simpleicons.org/selenium", CatTesting, []),
        new(175, "Cypress", "https://cdn.simpleicons.org/cypress", CatTesting, []),
        new(176, "Playwright", "https://cdn.simpleicons.org/playwright", CatTesting, []),
        new(177, "Jest", "https://cdn.simpleicons.org/jest", CatTesting, []),
        new(178, "Machine Learning", DefaultIconUri, CatDataScience,
        [
            new RawSimpleDefinition(226, "ML")
        ]),
        new(179, "Data Analysis", DefaultIconUri, CatDataScience, []),
        new(180, "Pandas", "https://cdn.simpleicons.org/pandas", CatDataScience, []),
        new(181, "NumPy", "https://cdn.simpleicons.org/numpy", CatDataScience, []),
        new(182, "TensorFlow", "https://cdn.simpleicons.org/tensorflow", CatDataScience, []),
        new(183, "PyTorch", "https://cdn.simpleicons.org/pytorch", CatDataScience, []),
        new(184, "Scikit-learn", "https://cdn.simpleicons.org/scikitlearn", CatDataScience, []),
        new(185, "Power BI", "https://cdn.simpleicons.org/powerbi", CatDataScience, []),
        new(186, "Tableau", "https://cdn.simpleicons.org/tableau", CatDataScience, []),
        new(187, "Agile Methodologies", DefaultIconUri, CatMethodologies,
        [
            new RawSimpleDefinition(227, "Agile")
        ]),
        new(188, "Scrum", "https://cdn.simpleicons.org/scrumalliance", CatMethodologies, []),
        new(189, "Kanban", DefaultIconUri, CatMethodologies, []),
        new(190, "JIRA", "https://cdn.simpleicons.org/jira", CatMethodologies, []),
        new(191, "Confluence", "https://cdn.simpleicons.org/confluence", CatMethodologies, []),
        new(192, "Trello", "https://cdn.simpleicons.org/trello", CatMethodologies, []),
        new(193, "Team Leadership", DefaultIconUri, CatSoftSkills, []),
        new(194, "Effective Communication", DefaultIconUri, CatSoftSkills, []),
        new(195, "Problem Solving", DefaultIconUri, CatSoftSkills, []),
        new(196, "Public Speaking", DefaultIconUri, CatSoftSkills, []),
        new(197, "Time Management", DefaultIconUri, CatSoftSkills, []),
        new(198, "Critical Thinking", DefaultIconUri, CatSoftSkills, []),
        new(199, "Mentoring", DefaultIconUri, CatSoftSkills, []),
        new(200, "Collaboration", DefaultIconUri, CatSoftSkills, [])
    ];

    public static int Count => Data.Count;
}