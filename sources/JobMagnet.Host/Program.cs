using JobMagnet.Application.Extensions;
using JobMagnet.Application.Services;
using JobMagnet.Host.Extensions;
using JobMagnet.Host.Services;
using JobMagnet.Infrastructure.Extensions;
using JobMagnet.Infrastructure.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAllowOrigins(builder.Configuration)
    .AddSqlServer<JobMagnetDbContext>(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        options =>
        {
            options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30),
                null);
        })
    .AddScoped<ICurrentUserService, HttpContextCurrentUserService>()
    .AddApplicationDependencies()
    .AddInfrastructureDependencies(builder.Configuration)
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