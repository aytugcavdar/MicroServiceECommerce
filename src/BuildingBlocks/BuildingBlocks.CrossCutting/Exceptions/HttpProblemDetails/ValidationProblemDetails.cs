using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace BuildingBlocks.CrossCutting.Exceptions.HttpProblemDetails;

public class ValidationProblemDetails : ProblemDetails
{
    public List<string> Errors { get; set; }

    public ValidationProblemDetails(List<string> errors)
    {
        Title = "Validation Error";
        Detail = "One or more validation errors occurred.";
        Status = StatusCodes.Status422UnprocessableEntity;
        Type = "https://example.com/probs/validation";
        Errors = errors;
    }
}