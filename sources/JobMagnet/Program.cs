using JobMagnet.Context;
using JobMagnet.DependencyInjection;
using JobMagnet.Extensions;

namespace JobMagnet;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddSqlServer<JobMagnetDbContext>(connectionString);
        builder.Services.AddControllers(options =>
        {
            options.InputFormatters.Insert(0, JsonPatchInputFormatter.GetJsonPatchInputFormatter());
        });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplicationServices();

        var app = builder.Build();

        // app.UseLoguearRespuestaHttp();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}