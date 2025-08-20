using System.Text;
using JobMagnet.Application.Extensions;
using JobMagnet.Application.Services;
using JobMagnet.Host.Extensions;
using JobMagnet.Host.Services;
using JobMagnet.Infrastructure.Extensions;
using JobMagnet.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAllowOrigins(builder.Configuration)
    .AddScoped<ICurrentUserService, HttpContextCurrentUserService>()
    .AddApplicationDependencies()
    .AddInfrastructureDependencies(builder.Configuration)
    .AddCorsPolicies(builder.Configuration)
    .AddHttpContextAccessor()
    .AddEndpointsApiExplorer()
    .AddApiVersion()
    .AddConfiguredOpenApi(builder.Configuration)
    .AddSwagger()
    .AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"] ?? string.Empty)),
        ClockSkew = TimeSpan.Zero
    });

var app = builder.Build();

if (builder.Configuration.GetValue<bool>("OpenApiSettings:UseUI"))
{
    app.UseScalar().UseSwagger();
}

app
    .UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization()
    .UseCors("DefaultCorsPolicy");
app.MapControllers();

await app.RunAsync();