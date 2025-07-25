using UnityEngine;

public class TemperatureValue : SensorProbe
{
    public float temperature = 0.0f; // Default temperature value
    public string description = ""; // Description of the material


    public override float Evaluate()
    {
        return temperature;
    }
}