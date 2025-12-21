using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Infrastructure.Outbox;

public static class EventTypeRegistry
{
    
    public static Dictionary<string, Type> Dictionary { get; } = new();

    
    public static void Register(string eventName, Type type)
    {
        if (!Dictionary.ContainsKey(eventName))
        {
            Dictionary.Add(eventName, type);
        }
    }
}
