using Director;
using GraphTool.Runtime;
using UnityEngine;

[ExecuteInEditMode]
public class SensorGraphDebug : MonoBehaviour
{
    public Sensor sensor;
    public Color trackColor;
    void OnEnable()
    {
        GraphSubscription.Subscribe(
            this,
            () => sensor.MinMaxNormalize(sensor.Evaluate()),
            trackColor
        );
    }
    
    void OnDisable()
    {
        GraphSubscription.Unsubscribe(this);
    }

    void OnDestroy()
    {
        GraphSubscription.Unsubscribe(this);
    }

}