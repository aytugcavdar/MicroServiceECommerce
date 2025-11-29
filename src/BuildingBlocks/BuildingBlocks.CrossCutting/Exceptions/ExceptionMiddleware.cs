using BuildingBlocks.CrossCutting.Exceptions.Handlers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.CrossCutting.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpExceptionHandler _httpExceptionHandler;

    // Middleware'ler Constructor'da RequestDelegate alır
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
        _httpExceptionHandler = new HttpExceptionHandler();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            // İsteği bir sonraki adıma ilet (Devam et)
            await _next(context);
        }
        catch (Exception exception)
        {
            // Hata olursa yakala ve Handler'a teslim et
            // (Loglama işlemini buraya ekleyeceğiz sonra)
            _httpExceptionHandler.Response = context.Response;
            await _httpExceptionHandler.HandleExceptionAsync(exception);
        }
    }
}
