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
    .AddCors(options =>
    {
        options.AddPolicy("AllowFrontend",
            builder => builder
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
    })
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"] ?? string.Empty)),
        ClockSkew = TimeSpan.Zero
    });


builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Description = "Authorization: Bearer {token}",
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {  new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "bearer",
                    Name = "Authorization",
                    In = ParameterLocation.Header
                },
                new List<string>()
            },
        });
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", politic => politic.RequireRole("Admin"));

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();

if (builder.Configuration.GetValue<bool>("OpenApiSettings:UseUI"))
{
    app.UseScalar().UseSwagger();
}

app
    .UseHttpsRedirection()
    .UseCors("AllowFrontend")
    .UseAuthentication()
    .UseAuthorization();
app.MapControllers();

await app.RunAsync();