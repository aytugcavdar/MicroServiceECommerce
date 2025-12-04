using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.CrossCutting.Exceptions.types;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string name, object key)
        : base($"{name} with id ({key}) was not found")
    {
    }
}
