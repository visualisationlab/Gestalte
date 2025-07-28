using System;
using System.Collections;
using UnityEngine;

public class MagnetMachine : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float maxRange = 10f;
    [SerializeField] private float pullSpeed = 5f;

    private void Start()
    {
        Physics2D.velocityIterations = 2; //TODO Replace to somewhere else
        Physics2D.positionIterations = 1;
    }

    void FixedUpdate()
    {
        Attract();
    }

    private void Attract()
    {
        Vector3 magnetPosition = transform.position;
        foreach (GameObject target in McGibbleTracker.GetAll())
        {
            if (target == null) continue; // Skip destroyed objects

            Vector3 direction = magnetPosition - target.transform.position;
            float distance = direction.magnitude;

            if (distance > maxRange) continue; // Skip far ones if desired
            direction.Normalize();

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * pullSpeed, ForceMode2D.Force);
            }
        }
    }
}
