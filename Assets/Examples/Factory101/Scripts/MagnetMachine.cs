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
    [Range(0f, 1f)][SerializeField] private float pullFraction = 0.1f; // how much closer per pull

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

        // Convert local radius to world radius accounting for scaling
        float scale = Mathf.Max(attractionArea.transform.lossyScale.x, attractionArea.transform.lossyScale.y);
        float worldRadius = attractionArea.radius * scale;

        Collider2D[] hits = Physics2D.OverlapCircleAll(magnetPos, worldRadius);
        Debug.Log($"[MagnetMachine] OverlapCircleAll found {hits.Length} colliders (worldRadius={worldRadius:F2})");

        foreach (var col in hits)
        {
            // Prefer component-based check rather than tag, safer and less error-prone
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

            float moveDistance = distance * pullFraction;
            Vector2 newPos = rb.position + toMagnet.normalized * moveDistance;

            // MovePosition should be used during physics steps; if this is called from outside FixedUpdate, it still queues it safely
            rb.MovePosition(newPos);
            Debug.Log($"[MagnetMachine] Pulled '{col.name}' closer by {moveDistance:F2} to {newPos}");
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
