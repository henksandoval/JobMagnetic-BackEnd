using Asp.Versioning.ApiExplorer;
using JobMagnet.Host.Extensions.SettingSections;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

// ReSharper disable ClassNeverInstantiated.Global
namespace JobMagnet.Host.Extensions;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwagger(this IServiceCollection service, IConfiguration configuration)
    {
        var swaggerSettings = configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();

        if (swaggerSettings is null ||
            string.IsNullOrWhiteSpace(swaggerSettings.Title) ||
            string.IsNullOrWhiteSpace(swaggerSettings.Description))
            throw new InvalidOperationException("SwaggerSettings is not set in the configuration.");

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
                    Title = swaggerSettings.Title,
                    Version = description.ApiVersion.ToString(),
                    Description = swaggerSettings.Description
                };

                options.SwaggerDoc(description.GroupName, info);
            }
        });

        return service;
    }

    internal static WebApplication UseOpenApi(this WebApplication application)
    {
        var swaggerSettings = application.Configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();

        if (swaggerSettings is null || string.IsNullOrWhiteSpace(swaggerSettings.Url))
            throw new InvalidOperationException("SwaggerSettings:Url is not set in the configuration.");

        application.UseSwagger(x =>
        {
            x.PreSerializeFilters.Add((openApiDocument, _) =>
            {
                openApiDocument.Servers = new List<OpenApiServer>
                {
                    new()
                    {
                        Url = swaggerSettings.Url
                    }
                };
            });
        });
        application.UseSwaggerUI(config =>
        {
            var groupNames = application.Services.GetRequiredService<IApiVersionDescriptionProvider>()
                .ApiVersionDescriptions
                .Select(x => x.GroupName);

            foreach (var groupName in groupNames)
                config.SwaggerEndpoint($"{swaggerSettings.Url}/swagger/{groupName}/swagger.json",
                    groupName.ToUpperInvariant());

            config.DocExpansion(DocExpansion.None);
            config.EnableTryItOutByDefault();
            config.DefaultModelRendering(ModelRendering.Example);
            config.EnableDeepLinking();
            config.DisplayRequestDuration();
        });

        return application;
    }
}

internal class LowerCaseDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = swaggerDoc.Paths.ToDictionary(entry => LowercaseEverythingButParameters(entry.Key),
            entry => entry.Value);

        swaggerDoc.Paths = new OpenApiPaths();

        foreach (var (key, value) in paths)
            swaggerDoc.Paths.Add(key, value);
    }

    private static string LowercaseEverythingButParameters(string key)
    {
        return string.Join('/', key.Split('/').Select(x => x.Contains('{') ? x : x.ToLower()));
    }
}

internal class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum) return;

        schema.Enum.Clear();

        Enum.GetNames(context.Type)
            .ToList()
            .ForEach(name => schema.Enum.Add(new OpenApiString($"{name}")));
    }
}