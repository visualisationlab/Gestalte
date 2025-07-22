using AIDirector;
using GraphTool.Runtime;
using UnityEngine;

public class SensorGraphDebug : MonoBehaviour
{
    public Sensor sensor;
    public Color trackColor;
    void Start()
    {
        GraphSubscription.Subscribe(
            this,
            () => sensor.MinMaxNormalize(sensor.Evaluate()),
            trackColor
        );
    }

    private void OnValidate()
    {
        GraphSubscription.Unsubscribe(this);
        GraphSubscription.Subscribe(
            this,
            () => sensor.MinMaxNormalize(sensor.Evaluate()),
            trackColor
        );
    }

    void OnDestroy()
    {
        GraphSubscription.Unsubscribe(this);
    }

}