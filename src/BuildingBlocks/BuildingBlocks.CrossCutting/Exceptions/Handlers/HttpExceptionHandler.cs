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

        return exception switch
        {
            BusinessException businessException => HandleException(businessException),
            BusinessValidationException validationException => HandleException(validationException),
            EntityNotFoundException entityNotFoundException => HandleException(entityNotFoundException),
            NotFoundException notFoundException => HandleException(notFoundException), 
            _ => HandleException(exception)
        };
    }

    private Task HandleException(BusinessException businessException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        var details = new BusinessProblemDetails(businessException.Message);
        return Response.WriteAsJsonAsync(details);
    }

    private Task HandleException(BusinessValidationException validationException)
    {
        Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        var details = new ValidationProblemDetails(validationException.Errors);
        return Response.WriteAsJsonAsync(details);
    }

    private Task HandleException(Exception exception)
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        var details = new InternalServerErrorProblemDetails(exception.Message);
        return Response.WriteAsJsonAsync(details);
    }


}