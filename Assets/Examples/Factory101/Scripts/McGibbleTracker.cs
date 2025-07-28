using System.Collections.Generic;
using UnityEngine;

public static class McGibbleTracker
{
    private static readonly HashSet<GameObject> trackedObjects = new();
    
    public static void Add(GameObject obj)
    {
        trackedObjects.Add(obj);
    }

    public static void Remove(GameObject obj)
    {
        trackedObjects.Remove(obj);
    }

    public static IEnumerable<GameObject> GetAll()
    {
        return trackedObjects;
    }

    public static int Count => trackedObjects.Count;
}
