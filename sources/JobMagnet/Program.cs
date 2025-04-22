using JobMagnet.Extensions;
using JobMagnet.Extensions.ConfigSections;
using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSettingSections(builder.Configuration)
    .AddSqlServer<JobMagnetDbContext>(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        options =>
        {
            options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30),
                null);
        })
    .AddHostDependencies()
    .AddCorsPolicies(builder.Configuration)
    .AddHttpContextAccessor()
    .AddEndpointsApiExplorer()
    .AddApiVersion()
    .AddSwagger(builder.Configuration)
    .AddControllers(options =>
    {
        options.InputFormatters.Insert(0, JsonPatchInputFormatter.GetJsonPatchInputFormatter());
    });

var app = builder.Build();

if (builder.Configuration.GetValue<bool>("SwaggerSettings:UseUI")) app.UseOpenApi();

app
    .UseHttpsRedirection()
    .UseAuthorization()
    .UseCors("DefaultCorsPolicy");
app.MapControllers();

await app.RunAsync();