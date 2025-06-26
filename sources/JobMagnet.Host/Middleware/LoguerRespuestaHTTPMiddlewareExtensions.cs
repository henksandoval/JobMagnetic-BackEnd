namespace JobMagnet.Host.Middleware;

public static class LoguerRespuestaHttpMiddlewareExtensions
{
    public static IApplicationBuilder UseLoguearRespuestaHttp(this IApplicationBuilder app) => app.UseMiddleware<LoguearRespuestaHttpMiddleware>();
}

public class LoguearRespuestaHttpMiddleware
{
    private readonly ILogger<LoguearRespuestaHttpMiddleware> logger;
    private readonly RequestDelegate siguiente;

    public LoguearRespuestaHttpMiddleware(RequestDelegate siguiente, ILogger<LoguearRespuestaHttpMiddleware> logger)
    {
        this.siguiente = siguiente;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        using (var memoryStream = new MemoryStream())
        {
            var cuerpoOriginalRespuesta = contexto.Response.Body;
            contexto.Response.Body = memoryStream;

            await siguiente(contexto);

            memoryStream.Seek(0, SeekOrigin.Begin);
            var respuesta = new StreamReader(memoryStream).ReadToEnd();
            memoryStream.Seek(0, SeekOrigin.Begin);

            await memoryStream.CopyToAsync(cuerpoOriginalRespuesta);
            contexto.Response.Body = cuerpoOriginalRespuesta;
            logger.LogInformation(respuesta);
        }
    }
}