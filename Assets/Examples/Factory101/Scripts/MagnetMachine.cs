using System;
using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class MagnetMachine : Machine, IPulseReceiver<bool>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float upgradePullMultiplier = 1.3f;
    public CircleCollider2D attractionArea;
    [SerializeField, Range(0f, 1f)] float pullFraction = 0.1f;
    [SerializeField] float velocityAccel = 10f; // how quickly the object chases the target velocity

    private void Start()
    {
        maxUpgradeLevel = 3;
    }

    protected override void RegisterLua()
    {
        UserData.RegisterType<MagnetMachine>();
        UserData.RegisterType<McGibbleDescription>(InteropAccessMode.Default);
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    public override string GetStatus()
    {
        return "Magnet Status";
    }

    public override string UpgradeMachine()
    {
        pullFraction = Mathf.Clamp01(pullFraction * upgradePullMultiplier);
        return $"Magnet upgraded! New pull fraction: {pullFraction}";
    }
    // private void FixedUpdate()
    // {
    //     // if “alwaysPull” OR we have remaining pull time, do one tick of attract
    //     if (alwaysPull || pullTimer > 0f)
    //     {
    //         Attract();

    //         // count down the timer if it’s active
    //         if (!alwaysPull)
    //         {
    //             pullTimer -= Time.fixedDeltaTime;
    //             if (pullTimer < 0f) pullTimer = 0f;
    //         }
    //     }
    // }

    [ExposeMethod("Pull objects towards the magnet")]
    public void Attract()
    {
        if (attractionArea == null)
        {
            Debug.LogWarning("[MagnetMachine] attractionArea is not assigned.");
            return;
        }

        Vector2 magnetPos = transform.position;

        float scale = Mathf.Max(attractionArea.transform.lossyScale.x, attractionArea.transform.lossyScale.y);
        float worldRadius = attractionArea.radius * scale;

        Collider2D[] hits = Physics2D.OverlapCircleAll(magnetPos, worldRadius);
        Debug.Log($"[MagnetMachine] OverlapCircleAll found {hits.Length} colliders (worldRadius={worldRadius:F2})");

        foreach (var col in hits)
        {
            McGibble target = col.GetComponent<McGibble>();
            if (target == null)
                continue;

            Rigidbody2D rb = col.attachedRigidbody;
            if (rb == null)
                continue;

            Vector2 toMagnet = magnetPos - rb.position;
            float distance = toMagnet.magnitude;
            if (distance <= Mathf.Epsilon)
                continue;

            // Desired displacement this tick (same as before)
            float moveDistance = distance * pullFraction;

            // Target velocity to achieve that displacement in one FixedUpdate
            float fixedDt = Time.fixedDeltaTime;
            Vector2 desiredVelocity = toMagnet.normalized * (moveDistance / fixedDt);

            // Smoothly approach desired velocity
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, desiredVelocity, velocityAccel * fixedDt);

            Debug.Log($"[MagnetMachine] Pulling '{col.name}' toward magnet. TargetVel={desiredVelocity:F2}, NewVel={rb.linearVelocity:F2}");
        }
    }

    [ContextMenu("Test Pull")]
    public void TestPull()
    {
        Attract();
    }

    public void OnPulse(bool message)
    {
        ExecuteScript();
    }
    
    private void OnDrawGizmosSelected()
    {
        if (attractionArea == null) return;
        Vector3 worldPos = transform.position;
        float scale = Mathf.Max(attractionArea.transform.lossyScale.x, attractionArea.transform.lossyScale.y);
        float worldRadius = attractionArea.radius * scale;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(worldPos, worldRadius);
    }
}
