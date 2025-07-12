using Asp.Versioning.ApiExplorer;
using JobMagnet.Host.Extensions.SettingSections;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace JobMagnet.Host.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddConfiguredOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        var openApiSettings = configuration.GetSection("OpenApiSettings").Get<OpenApiSettings>();

        if (openApiSettings is null ||
            string.IsNullOrWhiteSpace(openApiSettings.Title) ||
            string.IsNullOrWhiteSpace(openApiSettings.Description))
            throw new InvalidOperationException("OpenApiSettings is not configured.");

        var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
            services.AddOpenApi(description.GroupName, options => { options.AddDocumentTransformer<LowerCaseRoutesTransformer>(); });

        return services;
    }

    public static WebApplication UseConfiguredOpenApi(this WebApplication app)
    {
        var openApiSettings = app.Configuration.GetSection("OpenApiSettings").Get<OpenApiSettings>();
        if (openApiSettings is null || !openApiSettings.UseUi)
            return app;

        app.MapOpenApi();

        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle(openApiSettings.Title)
                .WithTheme(ScalarTheme.BluePlanet)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions.Reverse()) options.AddDocument(description.GroupName);
        });

        return app;
    }
}

public class LowerCaseRoutesTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var paths = document.Paths.ToDictionary(
            entry => string.Join('/', entry.Key.Split('/').Select(x => x.Contains('{') ? x : x.ToLowerInvariant())),
            entry => entry.Value
        );

        document.Paths = new OpenApiPaths();
        foreach (var (key, value) in paths) document.Paths.Add(key, value);

        return Task.CompletedTask;
    }
}

/*
public class EnumSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.Type.IsEnum)
        {
            schema.Type = "string"; // Asegura que el tipo base sea string
            schema.Enum.Clear();
            foreach (var name in Enum.GetNames(context.Type))
            {
                schema.Enum.Add(new OpenApiString(name));
            }
        }

        return Task.CompletedTask;
    }
}
*/