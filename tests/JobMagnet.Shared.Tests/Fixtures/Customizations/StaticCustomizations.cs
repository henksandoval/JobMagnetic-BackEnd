namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public static class StaticCustomizations
{
    public const string CvContent = """
                                    Laura Gómez Fernández
                                    (+34) 611-22-33-44 | laura.gomez.dev@example.net | linkedin.com/in/laura-gomez-fernandez | https://lauragomez.dev

                                    Resumen Profesional
                                    Ingeniera de Software con 7 años de experiencia en el desarrollo de aplicaciones web robustas y escalables. Especialista en el ecosistema .NET y Angular, con un historial probado en la entrega de proyectos de alta calidad, desde la concepción hasta el despliegue. Busco aplicar mis habilidades en arquitecturas de microservicios y soluciones en la nube para contribuir a proyectos innovadores.

                                    Educación
                                    Septiembre 2014 - Junio 2018
                                    Máster en Ingeniería de Software y Sistemas Informáticos
                                    Universidad Ficticia de Barcelona (UFB)

                                    Octubre 2010 - Julio 2014
                                    Grado en Ingeniería Informática
                                    Escuela Técnica Superior de Ingenieros Ficticia (ETSIF)

                                    Experiencia laboral
                                    Sr. Software Engineer .NET
                                    TechSolutions Global
                                    Marzo 2020 - Actualmente
                                    Diseño y desarrollo de componentes backend para una plataforma de e-commerce utilizando .NET Core, microservicios y Azure.
                                    Implementación de pipelines CI/CD con Azure DevOps para automatizar pruebas y despliegues.
                                    Colaboración en la definición de la arquitectura de nuevas funcionalidades y optimización de módulos existentes.
                                    Mentoría de desarrolladores junior y participación activa en revisiones de código.
                                    Tecnologias : (.NET Core, C#, ASP.NET Core, Microservices, Azure Functions, Azure Service Bus, Docker, Kubernetes, SQL Server, Cosmos DB, Git, Azure DevOps)

                                    Full Stack Developer
                                    Innovatech Digital
                                    Agosto 2018 - Febrero 2020
                                    Desarrollo full-stack de aplicaciones web para clientes del sector financiero y de seguros.
                                    Frontend desarrollado con Angular y TypeScript, consumiendo APIs RESTful construidas con ASP.NET Web API.
                                    Mantenimiento y evolución de sistemas legados, proponiendo mejoras y refactorizaciones.
                                    Participación en la planificación de sprints y ceremonias ágiles (Scrum).
                                    Tecnologias : (.NET Framework, ASP.NET MVC, Web API, C#, Entity Framework, Angular, TypeScript, JavaScript, HTML5, CSS3, SQL Server, JIRA)

                                    Certificaciones
                                    Microsoft Certified: Azure Developer Associate
                                    Microsoft - 2022 - Link_Azure_Cert
                                    Professional Scrum Master I (PSM I)
                                    Scrum.org - 2021 - Link_PSM_Cert
                                    Developing ASP.NET Core MVC Web Applications
                                    Pluralsight - 2020 - Link_Pluralsight_Course

                                    Habilidades Técnicas
                                    Lenguajes: C#, TypeScript, JavaScript, SQL
                                    Frameworks/Plataformas: .NET (Core, Framework), ASP.NET (Core, MVC, Web API), Entity Framework, Angular
                                    Bases de Datos: SQL Server, Cosmos DB, MongoDB (básico)
                                    Cloud: Azure (App Services, Functions, Service Bus, Kubernetes Service, DevOps)
                                    Herramientas: Docker, Kubernetes, Git, Visual Studio, VS Code, JIRA
                                    Otros: Microservicios, RESTful APIs, CI/CD, Metodologías Ágiles (Scrum, Kanban), DDD (conceptual)
                                    """;

    public static readonly List<(string Name, string IconClass)> ContactTypes =
    [
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
    ];

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
        "Analytical",
        "Innovative",
        "Leadership",
        "Time Management",
        "Critical Thinking",
        "Self-Motivated",
        "Resilient",
        "Empathetic",
        "Strategic Thinker",
        "Proactive",
        "Collaborative",
        "Results-Oriented",
        "Customer-Focused",
        "Tech-Savvy"
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

    public static readonly string[] Skills =
    [
        // --- Lenguajes de Programación ---
        "C#",
        "Java",
        "Python",
        "JavaScript",
        "TypeScript",
        "Go",
        "Rust",
        "Kotlin",
        "Swift",
        "PHP",
        "Ruby",
        "C++",
        "SQL",

        // --- Frameworks y Librerías (Backend) ---
        ".NET",
        "ASP.NET Core",
        "Spring Boot",
        "Django",
        "Flask",
        "Node.js",
        "Express.js",
        "Ruby on Rails",
        "Laravel",
        "Entity Framework Core",

        // --- Frameworks y Librerías (Frontend) ---
        "React",
        "Angular",
        "Vue.js",
        "Svelte",
        "Blazor",
        "jQuery",
        "HTML5",
        "CSS3",
        "Sass/SCSS",
        "Tailwind CSS",

        // --- Bases de Datos ---
        "SQL Server",
        "PostgreSQL",
        "MySQL",
        "Oracle Database",
        "MongoDB",
        "Redis",
        "Cassandra",
        "SQLite",
        "Cosmos DB",
        "DynamoDB",

        // --- Cloud y DevOps ---
        "Microsoft Azure",
        "Amazon Web Services (AWS)",
        "Google Cloud Platform (GCP)",
        "Docker",
        "Kubernetes",
        "Terraform",
        "Ansible",
        "Jenkins",
        "Azure DevOps",
        "GitHub Actions",
        "GitLab CI",
        "Prometheus",
        "Grafana",

        // --- Arquitectura y Patrones de Diseño ---
        "Microservices Architecture",
        "Domain-Driven Design (DDD)",
        "SOLID Principles",
        "REST APIs",
        "GraphQL",
        "Event-Driven Architecture",
        "CQRS",
        "Design Patterns",

        // --- Testing ---
        "Unit Testing",
        "Integration Testing",
        "End-to-End Testing",
        "xUnit",
        "NUnit",
        "MSTest",
        "Selenium",
        "Cypress",
        "Playwright",
        "Jest",

        // --- Data Science y Machine Learning ---
        "Machine Learning",
        "Data Analysis",
        "Pandas",
        "NumPy",
        "TensorFlow",
        "PyTorch",
        "Scikit-learn",
        "Power BI",
        "Tableau",

        // --- Metodologías y Herramientas de Gestión ---
        "Agile Methodologies",
        "Scrum",
        "Kanban",
        "JIRA",
        "Confluence",
        "Trello",

        // --- Habilidades Blandas (Soft Skills) ---
        "Team Leadership",
        "Effective Communication",
        "Problem Solving",
        "Public Speaking",
        "Time Management",
        "Critical Thinking",
        "Mentoring",
        "Collaboration"
    ];
}