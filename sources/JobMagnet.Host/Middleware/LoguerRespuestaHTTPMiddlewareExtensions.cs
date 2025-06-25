namespace JobMagnet.Host.Middleware;

public static class LoguerRespuestaHTTPMiddlewareExtensions
{
    public static IApplicationBuilder UseLoguearRespuestaHttp(this IApplicationBuilder app) => app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
}

public class LoguearRespuestaHTTPMiddleware
{
    private readonly ILogger<LoguearRespuestaHTTPMiddleware> logger;
    private readonly RequestDelegate siguiente;

    public LoguearRespuestaHTTPMiddleware(RequestDelegate siguiente, ILogger<LoguearRespuestaHTTPMiddleware> logger)
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