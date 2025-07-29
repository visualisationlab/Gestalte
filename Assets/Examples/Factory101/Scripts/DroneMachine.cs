using System.Collections;
using System.Collections.Generic;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using System.Linq;

public class DroneMachine : FilterMachine
{
    public MultiSimpleSensor grabArea;
    public Transform dropAreaCenter;
    public float dropRadius = 1.0f;

    [ExposeMethod("MoveGibbles")]
    public void MoveGibbles()
    {
        foreach (var obj in grabArea.detectedGameObjects.ToList())
        {
            if (obj == null || !IsAllowed(obj)) continue;

            Vector3 randomOffset = new Vector3(
                Random.Range(-dropRadius, dropRadius),
                Random.Range(-dropRadius, dropRadius),
                0f
            );

            obj.transform.position = dropAreaCenter.position + randomOffset;
        }
    }
}
