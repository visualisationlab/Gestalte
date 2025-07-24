using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agent
{
    public static class InstanceTracker
    {
        private static readonly Dictionary<Guid, GameObject> subscribers = new();
        
        public static string Subscribe(GameObject subscriber)
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
        
        public static string RetrieveGuid(GameObject mono)
        {
            if (mono == null)
            {
                throw new ArgumentNullException(nameof(mono), "MonoBehaviour cannot be null.");
            }
            foreach (var kvp in subscribers)
            {
                if (ReferenceEquals(kvp.Value, mono))
                {
                    return kvp.Key.ToString(); // Return existing GUID
                }
            }
            return null;
        }
    }
}