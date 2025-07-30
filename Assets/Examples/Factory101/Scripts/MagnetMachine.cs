using System;
using System.Collections;
using UnityEngine;

public class MagnetMachine : MonoBehaviour, IPulseReceiver<bool>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float maxRange = 10f;
    [SerializeField] private float pullSpeed = 5f;
    public bool alwaysPull = false;
    public float pullDuration = 1f; // Duration for which the magnet pulls objects
    // internal timer
    private float pullTimer = 0f;
    private void Start()
    {
        Physics2D.velocityIterations = 2; //TODO Replace to somewhere else
        Physics2D.positionIterations = 1;
    }

    private void FixedUpdate()
    {
        // if “alwaysPull” OR we have remaining pull time, do one tick of attract
        if (alwaysPull || pullTimer > 0f)
        {
            Attract();

            // count down the timer if it’s active
            if (!alwaysPull)
            {
                pullTimer -= Time.fixedDeltaTime;
                if (pullTimer < 0f) pullTimer = 0f;
            }
        }
    }

    private void Attract()
    {
        Vector3 magnetPosition = transform.position;
        foreach (McGibble target in McGibbleTracker.Instance.GetAll())
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

    public void OnPulse(bool message)
    {
        if (!message) return;
        pullTimer = pullDuration;
    }
}
