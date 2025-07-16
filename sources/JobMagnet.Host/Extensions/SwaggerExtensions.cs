using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace JobMagnet.Host.Extensions;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SchemaFilter<EnumSchemaFilter>();
            options.DocumentFilter<LowerCaseDocumentFilter>();
            options.UseInlineDefinitionsForEnums();

            var provider = services.BuildServiceProvider();
            var apiProvider = provider.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in apiProvider.ApiVersionDescriptions)
            {
                var info = new OpenApiInfo
                {
                    Title = $"JobMagnet API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                };

                options.SwaggerDoc(description.GroupName, info);
            }
        });

        return services;
    }

    internal static WebApplication UseSwagger(this WebApplication app)
    {
        SwaggerBuilderExtensions.UseSwagger(app);

        app.UseSwaggerUI(config =>
        {
            var apiProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in apiProvider.ApiVersionDescriptions)
            {
                config.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }

            config.DocExpansion(DocExpansion.None);
            config.EnableTryItOutByDefault();
            config.DefaultModelRendering(ModelRendering.Example);
        });

        return app;
    }
}

internal class LowerCaseDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = swaggerDoc.Paths.ToDictionary(
            entry => LowercaseEverythingButParameters(entry.Key),
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