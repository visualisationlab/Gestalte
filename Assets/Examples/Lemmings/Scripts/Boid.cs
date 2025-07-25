using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour
{
    public float speed = 2f;
    public float neighborRadius = 2f;
    public float separationRadius = 1f;
    public float maxForce = 0.5f;

    private Vector2 velocity;
    private FlockManager manager;

    public void Init(FlockManager manager)
    {
        this.manager = manager;
        velocity = Random.insideUnitCircle.normalized * speed;
    }

    void Update()
    {
        List<Boid> neighbors = manager.GetNearbyBoids(this, neighborRadius);

        Vector2 alignment = Vector2.zero;
        Vector2 cohesion = Vector2.zero;
        Vector2 separation = Vector2.zero;
        int count = 0;

        foreach (Boid other in neighbors)
        {
            if (other == this) continue;

            float dist = Vector2.Distance(transform.position, other.transform.position);
            alignment += other.velocity;
            cohesion += (Vector2)other.transform.position;

            if (dist < separationRadius)
                separation += ((Vector2)transform.position - (Vector2)other.transform.position) / dist;

            count++;
        }

        if (count > 0)
        {
            alignment = (alignment / count).normalized * speed - velocity;
            cohesion = ((cohesion / count) - (Vector2)transform.position).normalized * speed - velocity;
            separation = separation.normalized * speed - velocity;

            Vector2 steer = alignment + cohesion + separation;
            steer = Vector2.ClampMagnitude(steer, maxForce);
            velocity += steer;
        }

        velocity = velocity.normalized * speed;
        transform.position += (Vector3)(velocity * Time.deltaTime);

        // Face the direction of movement
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
