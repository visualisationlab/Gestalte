using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameObjectReference
{
    public GameObject key;
    public GameObject value;
}

[CreateAssetMenu(fileName = "GhostMachineReferences", menuName = "GhostMachineReferences")]
public class GhostMachineReference : ScriptableObject
{
    [SerializeField] private List<GameObjectReference> referenceList = new();

    // Runtime dictionary lazily built from the serialized list
    private Dictionary<GameObject, GameObject> _lookup;

    /// <summary>
    /// Try to get the value for a given key. Returns true if found.
    /// </summary>
    public bool TryGetTarget(GameObject key, out GameObject value)
    {
        EnsureLookupBuilt();
        return _lookup.TryGetValue(key, out value);
    }

    /// <summary>
    /// Convenience: get the target or null if missing.
    /// </summary>
    public GameObject GetTarget(GameObject key)
    {
        return TryGetTarget(key, out var value) ? value : null;
    }

    // Optional: helper to keep serialized list and runtime dictionary in sync
    public void Set(GameObject key, GameObject value)
    {
        for (int i = 0; i < referenceList.Count; i++)
        {
            if (referenceList[i].key == key)
            {
                var entry = referenceList[i];
                entry.value = value;
                referenceList[i] = entry;
                _lookup = null; // invalidate cache
                return;
            }
        }

        referenceList.Add(new GameObjectReference { key = key, value = value });
        _lookup = null;
    }

    private void EnsureLookupBuilt()
    {
        if (_lookup != null) return;
        _lookup = new Dictionary<GameObject, GameObject>();
        foreach (var pair in referenceList)
        {
            if (pair.key != null)
                _lookup[pair.key] = pair.value; // last wins on duplicates
        }
    }
}