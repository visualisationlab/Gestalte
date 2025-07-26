using UnityEngine;
using System.Collections.Generic;

public class FlockManager : MonoBehaviour
{
    [Header("Captain Settings")]
    public CaptainShip captain; 

    [Header("Boid Settings")]
    public Boid boidPrefab;
    public int boidCount = 30;
    public float spawnRadius = 5f;
    

    private List<Boid> boids = new List<Boid>();

    void Start()
    {
        for (int i = 0; i < boidCount; i++)
        {
            Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Boid boid = Instantiate(boidPrefab, pos, Quaternion.identity);
            boid.Init(this);
            boids.Add(boid);
        }
    }

    public List<Boid> GetNearbyBoids(Boid boid, float radius)
    {
        List<Boid> neighbors = new List<Boid>();
        foreach (Boid other in boids)
        {
            if (other != boid && Vector2.Distance(boid.transform.position, other.transform.position) < radius)
                neighbors.Add(other);
        }
        return neighbors;
    }
}
