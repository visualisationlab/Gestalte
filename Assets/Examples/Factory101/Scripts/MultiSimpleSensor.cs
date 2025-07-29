using System.Collections.Generic;
using UnityEngine;

public class MultiSimpleSensor : SimpleSensor
{
    public List<GameObject> detectedGameObjects = new();

    public override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col); // Optionally preserve SimpleSensor logic
        if (!detectedGameObjects.Contains(col.gameObject))
            detectedGameObjects.Add(col.gameObject);
    }

    public override void OnTriggerExit2D(Collider2D col)
    {
        base.OnTriggerExit2D(col); // Optionally preserve SimpleSensor logic
        detectedGameObjects.Remove(col.gameObject);
    }
}
