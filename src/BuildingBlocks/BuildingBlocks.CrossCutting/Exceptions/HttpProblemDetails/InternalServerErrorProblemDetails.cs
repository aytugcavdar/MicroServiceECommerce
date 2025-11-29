using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace BuildingBlocks.CrossCutting.Exceptions.HttpProblemDetails;

public class InternalServerErrorProblemDetails : ProblemDetails
{
    public InternalServerErrorProblemDetails(string detail)
    {
        Title = "Internal Server Error";
        Detail = "Internal Server Error";
        Status = StatusCodes.Status500InternalServerError;
        Type = "https://example.com/probs/internal";
        
    }
}
