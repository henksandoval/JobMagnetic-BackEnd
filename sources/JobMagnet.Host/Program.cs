using System.Text;
using JobMagnet.Application.Extensions;
using JobMagnet.Application.Services;
using JobMagnet.Application.UseCases.Auth.Ports.EmailDTO;
using JobMagnet.Domain.Aggregates;
using JobMagnet.Host.Extensions;
using JobMagnet.Host.Middlewares;
using JobMagnet.Host.Services;
using JobMagnet.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<AdminUserOptions>(builder.Configuration.GetSection("AdminUser"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // ValidIssuer = builder.Configuration["Jwt:Issuer"],
        // ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    });

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();

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