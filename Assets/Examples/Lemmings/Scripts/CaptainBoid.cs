using UnityEngine;
using Mediator;
using Director;

public class CaptainBoid : Boid
{
    [ExposeMethod("move away from the closest Sensor")]
    public void MoveAwayFromClosestSensor(float speed)
    {
        Sensor closestSensor = GetClosestSensor();
        if (closestSensor != null)
        {
            Vector2 direction = (Vector2)transform.position - (Vector2)closestSensor.transform.position;
            direction.Normalize();
            velocity += direction * speed;
        }
    }

    [ExposeMethod("move towards the closest Sensor")]
    public void MoveTowardsClosestSensor(float speed)
    {
        Sensor closestSensor = GetClosestSensor();
        if (closestSensor != null)
        {
            Vector2 direction = closestSensor.transform.position - transform.position;
            direction.Normalize();
            velocity += direction * speed;
        }
    }

    private Sensor GetClosestSensor()
    {
        // Get all gameobjects with the Sensor component
        Sensor[] sensors = FindObjectsOfType<Sensor>();
        Sensor closestSensor = null;

        // Get closest sensor
        float closestDistance = float.MaxValue;
        foreach (Sensor sensor in sensors)
        {
            float distance = Vector2.Distance(transform.position, sensor.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSensor = sensor;
            }
        }
        return closestSensor;
    }
}
