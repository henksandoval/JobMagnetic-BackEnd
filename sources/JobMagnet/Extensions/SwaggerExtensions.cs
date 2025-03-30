using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace JobMagnet.Extensions;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwagger(this IServiceCollection service)
    {
        var provider = service.BuildServiceProvider();
        var apiProvider = provider.GetRequiredService<IApiVersionDescriptionProvider>();

        service.AddSwaggerGen(options =>
        {
            options.SchemaFilter<EnumSchemaFilter>();
            options.DocumentFilter<LowerCaseDocumentFilter>();
            options.UseInlineDefinitionsForEnums();

            foreach (var description in apiProvider.ApiVersionDescriptions)
            {
                var info = new OpenApiInfo
                {
                    Title = "JobMagnet API",
                    Version = description.ApiVersion.ToString(),
                    Description = "Public API JobMagnet",
                };

                options.SwaggerDoc(description.GroupName, info);
            }

            AddXmlDocumentation(options);
        });

        return service;
    }

    internal static WebApplication UseOpenApi(this WebApplication application)
    {
        var url = application.Configuration.GetValue<string>("SwaggerUrlApi");

        application.UseSwagger(x =>
        {
            x.PreSerializeFilters.Add((openApiDocument, request) =>
            {
                openApiDocument.Servers = new List<OpenApiServer>
                {
                    new()
                    {
                        Url = url
                    }
                };
            });
        });
        application.UseSwaggerUI(config =>
        {
            var groupNames = application.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions
                .Select(x => x.GroupName);

            foreach (var groupName in groupNames)
            {
                config.SwaggerEndpoint($"{url}/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant());
                config.DocExpansion(DocExpansion.None);
                config.EnableTryItOutByDefault();
            }

            config.DefaultModelRendering(ModelRendering.Example);
        });

        return application;
    }

    private static void AddXmlDocumentation(SwaggerGenOptions options)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        var files = directory.GetFiles("*.xml");

        foreach (var file in files)
        {
            var xmlPath = Path.Combine(AppContext.BaseDirectory, file.Name);

            options.IncludeXmlComments(xmlPath);
            options.CustomSchemaIds(x => x.FullName);
        }
    }
}

internal class LowerCaseDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = swaggerDoc.Paths.ToDictionary(entry => LowercaseEverythingButParameters(entry.Key), entry => entry.Value);

        swaggerDoc.Paths = new OpenApiPaths();

        foreach (var (key, value) in paths)
            swaggerDoc.Paths.Add(key, value);
    }

    private static string LowercaseEverythingButParameters(string key) =>
        string.Join('/', key.Split('/').Select(x => x.Contains('{') ? x : x.ToLower()));
}

internal class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();

            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(name => schema.Enum.Add(new OpenApiString($"{name}")));
        }
    }
}