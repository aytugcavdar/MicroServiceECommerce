using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace BuildingBlocks.CrossCutting.Exceptions.HttpProblemDetails;

public class BusinessProblemDetails : ProblemDetails
{
    public BusinessProblemDetails(string detail)
    {
        Title = "Business Rule Violation";
        Detail = detail;
        Status = StatusCodes.Status400BadRequest;
        Type = "https://example.com/probs/business";
    }
}
