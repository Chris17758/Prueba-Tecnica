using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "An unhandled exception occurred.");

        // Configurar la respuesta de la API
        context.Result = new ObjectResult(new
        {
            StatusCode = 500,
            Message = "Ocurrió un error interno en el servidor."
        })
        {
            StatusCode = 500,
            ContentTypes = { "application/json" }
        };
    }
}
