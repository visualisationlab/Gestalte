using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float maxForce = 0.5f;

    [Header("Boid Behavior Radii")]
    public float neighborRadius = 2f;
    public float separationRadius = 1f;

    [Header("Behavior Weights")]
    public float alignWeight = 1.0f;
    public float cohesionWeight = 0.8f;
    public float separationWeight = 2.0f;
    public float captainWeight = 1.5f;
    public float orbitWeight = 1.0f;
    public float noiseWeight = 2f; // Weight for noise influence
    private float noiseOffset;

    protected Vector2 velocity;
    private FlockManager manager;

    public void Init(FlockManager manager)
    {
        this.manager = manager;
        velocity = Random.insideUnitCircle.normalized * speed;
        noiseOffset = Random.Range(0f, 1000f); // Unique per boid
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
        }

        // Captain influence
        Vector2 toCaptain = (manager.captain.transform.position - transform.position);
        Vector2 towardCaptain = toCaptain.normalized;
        Vector2 aroundCaptain = new Vector2(-towardCaptain.y, towardCaptain.x); // Perpendicular
        Vector2 orbitForce = aroundCaptain * orbitWeight;

        // Noise force
        float noiseTime = Time.time + noiseOffset;
        float noiseAngle = Mathf.PerlinNoise(noiseTime, noiseTime * 0.5f) * Mathf.PI * 2f;
        Vector2 noiseDirection = new Vector2(Mathf.Cos(noiseAngle), Mathf.Sin(noiseAngle));
        Vector2 noiseForce = noiseDirection * noiseWeight; // Tweak this value

        // Combine forces
        Vector2 steer =
            alignment * alignWeight +
            cohesion * cohesionWeight +
            separation * separationWeight +
            towardCaptain * captainWeight +
            orbitForce +
            noiseForce;

        steer = Vector2.ClampMagnitude(steer, maxForce);
        velocity += steer;
        velocity = velocity.normalized * speed;
        transform.position += (Vector3)(velocity * Time.deltaTime);

        float targetAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
