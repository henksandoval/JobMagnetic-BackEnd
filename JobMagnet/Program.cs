
using JobMagnet.Context;
using JobMagnet.DependencyInjection;
using JobMagnet.Middleware;
using JobMagnet.Service;
using JobMagnet.Service.Interface;

namespace JobMagnet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddSqlServer<JobMagnetDbContext>(builder.Configuration.GetConnectionString("defaultConnection"));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplicationServices();

            var app = builder.Build();

            app.UseLoguearRespuestaHttp();
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
}
