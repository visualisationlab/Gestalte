using System;
using System.Collections.Generic;

namespace Agent
{
    public static class InstanceTracker
    {
        private static readonly Dictionary<Guid, object> subscribers = new();
        
        public static string Subscribe(object subscriber)
        {
            // Check if the object already exists
            foreach (var kvp in subscribers)
            {
                if (ReferenceEquals(kvp.Value, subscriber))
                {
                    return kvp.Key.ToString(); // Return existing GUID
                }
            }
            
            Guid guid = Guid.NewGuid();
            subscribers.Add(guid, subscriber);
            return guid.ToString();
        }

        public static object Retrieve(string guid)
        {
            if (string.IsNullOrEmpty(guid) || !Guid.TryParse(guid, out var parsedGuid))
            {
                throw new ArgumentException("Invalid GUID format.");
            }
            subscribers.TryGetValue(parsedGuid, out var subscriber);
            if (subscriber == null)
            {
                throw new KeyNotFoundException($"No subscriber found for GUID: {guid}");
            }
            return subscriber;
        }
    }
}