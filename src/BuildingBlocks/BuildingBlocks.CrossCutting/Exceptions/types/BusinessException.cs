using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.CrossCutting.Exceptions.types;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
}