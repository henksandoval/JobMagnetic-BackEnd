using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Application.UseCases.Auth.Ports;
using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Infrastructure.ExternalServices.Identity.Entities;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Services.Auth;
using JobMagnet.Infrastructure.Services.CvParsers;
using JobMagnet.Shared.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<JobMagnetDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
                });
        });

        services.AddIdentity<ApplicationIdentityUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<JobMagnetDbContext>()
            .AddDefaultTokenProviders();
        services
            .AddSharedDependencies()
            .AddTransient<IRawCvParser, GeminiCvParser>()
            .AddTransient<IUserManagerAdapter, UserManagerAdapter>()
            .AddPersistence()
            .AddGemini(configuration);
        
        return services;
    }
}