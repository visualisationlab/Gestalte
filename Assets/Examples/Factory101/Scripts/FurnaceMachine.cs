using System;
using System.Collections;
using Examples.Factory101.Scripts;
using Examples.Factory101.Scripts.Input;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using Coroutine = UnityEngine.Coroutine;
using Random = UnityEngine.Random;

public class FurnaceMachine : Machine, IBuyable, IBlockPlacement
{
    public SimpleSensor sensor;
    public Transform outputPoint;
    public int heat = 100;
    private Vector3 tinyRandom;
    public float stability = 0.5f;
    
    [SerializeField] private float blastRate;
    private float maxBlastRate = 1f;
    
    private Coroutine loopRoutine;

    protected override void RegisterLua()
    {
        UserData.RegisterType<FurnaceMachine>();
        UserData.RegisterType<McGibbleDescription>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
        luaScript.Globals["McGibbleDescription"] = UserData.CreateStatic<McGibbleDescription>();
        loopRoutine = StartCoroutine(ExecuteEverySecond());
    }
    protected override void AfterSetScript()
    {
        ExecuteScript();
        RestartCoroutine();
    }
    private void RestartCoroutine()
    {
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
        }

        loopRoutine = StartCoroutine(ExecuteEverySecond());
    }    
    public override string GetStatus()
    {
        string result = "";
        result += $"Level: {upgradeLevel}/{maxUpgradeLevel} \n";
        result += $"Temperature: {heat} (min:30, max:250)\n";
        result += $"Rate: {GetBlastRate()} item(s)/s (max {GetMaxBlastRate()})\n";
        result += $"Stability: {stability})\n";
        return result;
    }

    public override void UpgradeMachine()
    {
        upgradeLevel++;
        
        maxBlastRate = upgradeLevel switch
        {
            2 => 3f,
            3 => 5f,
            4 => 6f,
            _ => 1f // default case
        };
        
        stability = upgradeLevel switch
        {
            2 => .5f,
            3 => .8f,
            4 => 1f,
            _ => 0.2f // default case
        };
    }

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            Blast();
            yield return new WaitForSeconds(1f / blastRate);
        }
    }

    [ExposeMethod("Sets the temperature of the furnace")]
    public void SetTemperature(int heat)
    {
        this.heat = Math.Clamp(heat, 30, 250);
    }
    
    [ExposeMethod("Sets how many items this machine heats every second")]
    public void SetMixRate(float rate)
    {
        blastRate = Mathf.Min(rate, maxBlastRate);
    }

    public void Blast()
    {
        if (!sensor.detectedGameObject) return;

        var mcGibble = sensor.detectedGameObject.GetComponent<McGibble>();
        if (mcGibble == null) return;

        // Stability in [0,1], where 1 = perfectly steady (no random wobble)
        float stabilityClamped = Mathf.Clamp01(stability);

        // Affinity is normalized [0,1]: 1 means exact furnace heat, lower values undershoot
        float affinity = Mathf.Clamp01(mcGibble.description.normalizedHeatAffinity);

        // Base resistance: affinity=1 => multiplier 1; affinity=0 => undershoot by up to maxUndershoot
        const float maxUndershoot = 0.3f; // how far below 1 it can go
        float baseResistance = 1f - (1f - affinity) * maxUndershoot; // in [1 - maxUndershoot, 1]

        // Add jitter scaled down by stability (no overshoot from noise, just wiggle)
        const float maxVariation = 0.15f; // maximum noise when stability is zero
        float noise = Random.Range(-1f, 1f) * maxVariation * (1f - stabilityClamped);

        float randomizedHeatResistance = Mathf.Max(0f, baseResistance + noise);
        float adjustedHeat = heat * randomizedHeatResistance;

        // Clamp to sane bounds if needed
        adjustedHeat = Mathf.Clamp(adjustedHeat, -100f, 500f);
        mcGibble.heat = Mathf.RoundToInt(adjustedHeat);

        tinyRandom = new Vector3(Random.value, Random.value - 0.5f, 0f);
        mcGibble.transform.position = outputPoint.transform.position + tinyRandom;
    }

    public int GetPrice()
    {
        return basePrice;
    }

    public string GetDescription()
    {
        return description;
    }
    
    private string GetBlastRate()
    {
        return blastRate.ToString("0.0");
    }

    private string GetMaxBlastRate()
    {
        return maxBlastRate.ToString("0.0");
    }
}
