using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using TarjetaCPruebaAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración del DbContext de Entity Framework Core
builder.Services.AddDbContext<CreditCardDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLSERVER")));

// Agregar servicios al contenedor
builder.Services.AddControllers(options =>
{
    // Agregar filtro de excepciones globalmente
    options.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddControllers(options => {
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

// Agregar generadores de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();

// Filtro de excepciones personalizado
public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var logger = context.HttpContext.RequestServices.GetService<ILogger<GlobalExceptionFilter>>();

        // Verificar que logger no sea nulo antes de usarlo
        if (logger != null)
        {
            logger.LogError(context.Exception, $"Something went wrong: {context.Exception.Message}");
        }

        context.Result = new ObjectResult(new { error = "Internal Server Error" })
        {
            StatusCode = 500
        };
    }
}

