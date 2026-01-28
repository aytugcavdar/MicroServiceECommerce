using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.CrossCutting.Exceptions.types;

public class EntityNotFoundException : NotFoundException
{
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} with identifier '{key}' was not found.")
    {
        EntityName = entityName;
        Key = key;
    }

    public string EntityName { get; }
    public object Key { get; }
}