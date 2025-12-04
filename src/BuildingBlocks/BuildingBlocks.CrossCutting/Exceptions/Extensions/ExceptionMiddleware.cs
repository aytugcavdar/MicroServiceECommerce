using BuildingBlocks.CrossCutting.Exceptions.Handlers;
using BuildingBlocks.CrossCutting.Exceptions.types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Serilog.Context;

namespace BuildingBlocks.CrossCutting.Exceptions.Extensions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpExceptionHandler _httpExceptionHandler;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
        _httpExceptionHandler = new HttpExceptionHandler();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            // ============================================
            // LOGLAMA - KENDİ AÇIKLAMAN
            // ============================================

            // LogContext.PushProperty ile log'a ekstra bilgiler ekle
            // Bu bilgiler sadece bu catch bloğunda geçerli olacak
            using (LogContext.PushProperty("RequestPath", context.Request.Path))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            using (LogContext.PushProperty("UserAgent", context.Request.Headers["User-Agent"].ToString()))
            using (LogContext.PushProperty("RemoteIP", context.Connection.RemoteIpAddress?.ToString()))
            {
                // Eğer kullanıcı giriş yapmışsa ID'sini de ekle
                var userId = context.User?.FindFirst("sub")?.Value
                          ?? context.User?.FindFirst("nameidentifier")?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    using (LogContext.PushProperty("UserId", userId))
                    {
                        LogException(exception);
                    }
                }
                else
                {
                    LogException(exception);
                }
            }

            // Hatayı HTTP response olarak dön
            _httpExceptionHandler.Response = context.Response;
            await _httpExceptionHandler.HandleExceptionAsync(exception);
        }
    }

    private void LogException(Exception exception)
    {
        // Exception tipine göre farklı seviyede log
        switch (exception)
        {
            // Business hataları: Kullanıcının yanlış bir şey yapması
            // Örnek: "Email already exists", "Product not found"
            case BusinessException businessException:
                Log.Warning(businessException,
                    "Business rule violation: {Message}",
                    businessException.Message);
                break;

            // Validation hataları: Form doğrulama hataları
            // Örnek: "Email is required", "Price must be greater than 0"
            case BusinessValidationException validationException:
                Log.Warning(validationException,
                    "Validation failed: {Errors}",
                    string.Join(", ", validationException.Errors));
                break;

            // Diğer hatalar: Beklenmedik sistem hataları
            // Örnek: NullReferenceException, DatabaseConnectionException
            default:
                Log.Error(exception,
                    "Unhandled exception occurred: {ExceptionType} - {Message}",
                    exception.GetType().Name,
                    exception.Message);
                break;
        }
    }
}
