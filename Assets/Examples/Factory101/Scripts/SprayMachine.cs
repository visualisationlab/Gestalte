using System;
using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.Events;

public class SprayMachine : Machine, IPulseReceiver<McGibbleDescription>, IPulseReceiver<bool>
{
    [Header("Detection & Output")]
    public SimpleSensor sensor;
    public Transform outputPoint;

    [Header("Spray Settings")]
    public Color sprayColor = Color.magenta;
    public FinishType finishType = FinishType.Mat;
    [Range(0f, 1f)] public float intensity = 1f; // 0 = no change, 1 = full spray

    [Header("Runtime")]
    public UnityEvent onStartSpray;
    public UnityEvent onStopSpray;
    private McGibbleDescription lastNotifiedMcGibble;

    private void Start()
    {
        maxUpgradeLevel = Enum.GetValues(typeof(FinishType)).Length - 2;
        StartCoroutine(ExecuteEverySecond());
    }

    protected override void RegisterLua()
    {
        UserData.RegisterType<SprayMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    public override string GetStatus() => "Spray Machine Status";

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            ExecuteScript();
            yield return new WaitForSeconds(1f);
        }
    }

    [ExposeMethod("Set the spray color (r,g,b) in 0.0 - 1.0 range")]
    public void SetSprayColor(float r, float g, float b)
    {
        sprayColor = new Color(r, g, b);
        Debug.Log($"[SprayMachine] SetSprayColor called. New sprayColor = {sprayColor} (r={r}, g={g}, b={b})");
    }

    [ExposeMethod("Set spray intensity (0 = no effect, 1 = full color)")]
    public void SetIntensity(float val)
    {
        intensity = Mathf.Clamp01(val);
    }

    [ExposeMethod("Trigger a spray on the currently detected item")]
    public void Spray()
    {
        if (!sensor.onDetect || sensor.detectedGameObject == null)
        {
            Debug.LogWarning("[SprayMachine] No object detected to spray.");
            return;
        }

        onStartSpray?.Invoke();

        var mcGibble = sensor.detectedGameObject.GetComponent<McGibble>();
        if (outputPoint != null)
        {
            mcGibble.transform.position = outputPoint.position + new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0f);
        }

        // assign finish before applying spray
        if (mcGibble != null)
            mcGibble.SetFinishType(finishType);

        ApplySpray(mcGibble);

        onStopSpray?.Invoke();
    }

    private void ApplySpray(McGibble mcGibble)
    {
        var obj = mcGibble.gameObject;
        var textMesh = obj.GetComponentInChildren<TextMeshPro>();
        if (textMesh != null)
        {
            textMesh.color = Color.Lerp(textMesh.color, sprayColor, intensity);
        }
    }

    // Pulse receiver: trigger spray on true pulse
    public void OnPulse(bool pulse)
    {
        if (pulse)
        {
            Spray();
        }
    }

    [ContextMenu("Test spray manually")]
    public void TestSpray()
    {
        Spray();
    }

    public void OnPulse(McGibbleDescription mcGibble)
    {
        lastNotifiedMcGibble = mcGibble;
    }

    public override string UpgradeMachine()
    {

        // Based on the current upgrade level we can change the spray finish type
        if (MaxUpgradeLevelReached())
        {
            finishType = FinishType.Galactic;
        }
        else
        {
            finishType = GetNextFinishType(finishType);
            this.upgradedAmount++;
        }

        Debug.Log($"SprayMachine upgraded to {finishType}");
        return $"SprayMachine upgraded to {finishType}";
    }
    
    private FinishType GetNextFinishType(FinishType current)
    {
        var values = (FinishType[])Enum.GetValues(typeof(FinishType));
        int idx = Array.IndexOf(values, current);
        idx = (idx + 1) % values.Length; // wraps around
        return values[idx];
    }

}
