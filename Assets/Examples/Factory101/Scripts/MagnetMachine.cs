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
    private Coroutine pullRoutine;
    private void Start()
    {
        Physics2D.velocityIterations = 2; //TODO Replace to somewhere else
        Physics2D.positionIterations = 1;
    }

    void FixedUpdate()
    {
        if (alwaysPull)
            Attract();
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

        // If already pulling, stop that coroutine
        if (pullRoutine != null)
            StopCoroutine(pullRoutine);

        pullRoutine = StartCoroutine(PullGibbles());
    }

    private IEnumerator PullGibbles()
    {
        float timer = 0f;
        while (timer < pullDuration)
        {
            Attract();
            timer += Time.deltaTime;
            yield return null;
        }
        pullRoutine = null;
    }
}
