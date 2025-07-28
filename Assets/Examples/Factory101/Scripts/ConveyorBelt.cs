using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 1f;
    public float beltLength = 5f;
    public float minItemSpacing = 0.5f;
    public GameObject mockPrefab;
    public ConveyorBelt neighbor;
    public class BeltItem
    {
        public GameObject visual;
        public float position; // 0 = start, beltLength = end
    }

    private List<BeltItem> items = new();

    void Update()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            var item = items[i];
            var newPosition = item.position + speed * Time.deltaTime;
            if (!HasSpace(newPosition, item)) continue;
            item.position = newPosition;
            item.visual.transform.localPosition = StartPosition() + (Vector3.right * item.position);
            if (item.position >= beltLength)
            {
                if(neighbor)
                {
                    float spawnPosition = NeighbourIsPerpendicular() ? 0.5f : 0f;
                    if(neighbor.HasSpace(spawnPosition)){
                        neighbor.TakeOverItem(item.visual, spawnPosition);
                        items.RemoveAt(i);
                    }
                    else
                    {
                        item.position = 1f;
                    }
                }
                else
                {
                    item.position = 1f;
                }
            }
        }
    }

    private bool NeighbourIsPerpendicular()
    {
        Vector3 myDirection = transform.right;
        Vector3 neighborDirection = neighbor.transform.right;
        float dot = Vector3.Dot(myDirection.normalized, neighborDirection.normalized);
        return Mathf.Abs(dot) < 0.01f; // Allow for small float imprecision
    }

    public bool HasSpace(float position, BeltItem ignore = null)
    {
        foreach (var item in items)
        {
            if (item == ignore) continue; // skip self
            if (Mathf.Abs(item.position - position) <= minItemSpacing)
            {
                return false;
            }
        }
        return true;
    }



    private Vector3 StartPosition()
    {
        return Vector3.left * 0.5f;
    }
    
    [ContextMenu("Mock Add")]
    public void MockAdd()
    {
        InsertItem(mockPrefab, 0f);
    }
    
    public void InsertItem(GameObject visualPrefab, float position = 0f)
    {
        if (!HasSpace(position)) return;
        var visual = Instantiate(visualPrefab);
        TakeOverItem(visual);
    }
    
    public void TakeOverItem(GameObject incoming, float position = 0f)
    {
        if (!HasSpace(position)) return;
        // Make sure it's in the correct space
        incoming.transform.SetParent(transform);
        incoming.transform.localPosition = StartPosition() + Vector3.right * position;
        items.Add(new BeltItem { visual = incoming, position = position});
        items.Sort((a, b) => a.position.CompareTo(b.position));
    }
}