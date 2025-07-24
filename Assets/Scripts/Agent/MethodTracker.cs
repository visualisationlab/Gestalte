using System;
using System.Collections.Generic;
using System.Reflection;

namespace Agent
{
    public static class MethodTracker
    {
        private static Dictionary<Guid, (MethodInfo Method, object Target)> registry = new();

        public static Guid Subscribe(MethodInfo method, object target)
        {
            var id = Guid.NewGuid();
            registry[id] = (method, target);
            return id;
        }

        public static MethodInfo RetrieveMethodInfo(string guid)
        {
            return registry.TryGetValue(Guid.Parse(guid), out var entry) 
                ? entry.Method 
                : throw new KeyNotFoundException($"No method found for GUID: {guid}");
        }
        
        public static void Invoke(string guid, object[] parameters)
        {
            if (!registry.TryGetValue(Guid.Parse(guid), out var entry))
                throw new InvalidOperationException("No method registered for this ID");

            entry.Method.Invoke(entry.Target, parameters); 
        }
    }
}