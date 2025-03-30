using JobMagnet.DependencyInjection;
using JobMagnet.Extensions;
using JobMagnet.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
    .AddSqlServer<JobMagnetDbContext>(connectionString)
    .AddHostDependencies()
    .AddHttpContextAccessor()
    .AddEndpointsApiExplorer()
    .AddApiVersion()
    .AddSwagger()
    .AddControllers(options =>
    {
        options.InputFormatters.Insert(0, JsonPatchInputFormatter.GetJsonPatchInputFormatter());
    });


var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseOpenApi();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();