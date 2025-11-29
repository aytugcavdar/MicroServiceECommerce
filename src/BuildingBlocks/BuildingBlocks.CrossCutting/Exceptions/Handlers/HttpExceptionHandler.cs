using BuildingBlocks.CrossCutting.Exceptions.HttpProblemDetails;
using BuildingBlocks.CrossCutting.Exceptions.types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.CrossCutting.Exceptions.Handlers;

public class HttpExceptionHandler
{
    private HttpResponse? _response;

    public HttpResponse Response
    {
        get => _response ?? throw new ArgumentNullException(nameof(_response));
        set => _response = value;
    }

    public Task HandleExceptionAsync(Exception exception)
    {
        Response.ContentType = "application/json";

        // Hatanın tipine göre işlem yap
        return exception switch
        {
            BusinessException businessException => HandleException(businessException),
            _ => HandleException(exception) // Tanımadığımız diğer tüm hatalar
        };
    }

    private Task HandleException(BusinessException businessException)
    {
        // HTTP 400 yap
        Response.StatusCode = StatusCodes.Status400BadRequest;

        // JSON formatını hazırla
        var details = new BusinessProblemDetails(businessException.Message);

        // JSON olarak yaz
        return Response.WriteAsJsonAsync(details);
    }

    private Task HandleException(Exception exception)
    {
        // HTTP 500 yap
        Response.StatusCode = StatusCodes.Status500InternalServerError;

        var details = new InternalServerErrorProblemDetails(exception.Message);
        return Response.WriteAsJsonAsync(details);
    }
}
