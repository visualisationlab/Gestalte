using System;
using AIDirector;
using GraphTool.Runtime;
using UnityEngine;

public class CorrelatorGraphDebug : MonoBehaviour
{
    public string name;
    public Correlator correlator;
    public Color trackColor;
    void Start()
    {
        // “MyValue” is the series ID shown in the legend
        // () => MyComponent.SomeValue must return 0–100 (it will be clamped)
        // Color.yellow is the curve color
        GraphSubscription.Subscribe(
            name,
            () => correlator.Evaluate() * 100f,
            trackColor
        );
    }

    private void OnValidate()
    {
        GraphSubscription.Unsubscribe(name);
        GraphSubscription.Subscribe(
            name,
            () => correlator.Evaluate() * 100f,
            trackColor
        );
    }

    void OnDestroy()
    {
        GraphSubscription.Unsubscribe(name);
    }

}
