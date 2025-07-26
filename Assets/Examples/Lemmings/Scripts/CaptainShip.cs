using UnityEngine;
using Mediator;
using Director;
using Agent;
public class CaptainShip : MonoBehaviour
{
    public float drag = 0.99f; // Multiplier for slowing down, e.g., 0.95 = 5% speed loss per frame
    public float maxSpeed = 1f;

    [Header("Agent Settings")]
    public bool startPrompting = false;
    public AgentController agentController;
    [Range(5f, 10.0f)]
    public float execPromptSeconds = 5.0f; // Time to wait before executing the next command

    private Vector2 velocity;

    public void Init(FlockManager manager)
    {
    }


    // [ExposeMethod("Push ship in direction")]
    public void PushShipInDirectionVector2(Vector2 direction, float thrust)
    {
        direction.Normalize();
        velocity += direction * thrust;

        // Optional: Clamp to max speed
        if (velocity.magnitude > maxSpeed)
            velocity = velocity.normalized * maxSpeed;
    }

    [ExposeMethod("Push ship in direction with angle in degrees")]
    public void PushShipInDirection(float angleInDegrees, float thrust)
    {
        Vector2 direction = new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
        PushShipInDirectionVector2(direction, thrust);
    }

    [ContextMenu("Test push ship in direction")]
    public void TestPushShipInDirection()
    {
        Vector2 testDirection = Random.insideUnitCircle.normalized * maxSpeed;
        float testThrust = 1.0f; // Example thrust
        PushShipInDirectionVector2(testDirection, testThrust);
    }

    void Update()
    {
        // Every execPromptSeconds, execute the next command
        if (agentController != null && startPrompting && Time.time % execPromptSeconds < Time.deltaTime)
        {
            agentController.ExecutePrompt();
        }

        // Move the ship
        transform.position += (Vector3)(velocity * Time.deltaTime);

        // Apply drag
        // velocity *= drag;

        // Optional: Face direction of movement
        if (velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 5f);
        }
    }

    private Sensor GetClosestSensor()
    {
        Sensor[] sensors = FindObjectsOfType<Sensor>();
        Sensor closestSensor = null;
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
