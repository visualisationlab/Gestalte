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
    [SerializeField] private float maxBlastRate = 1f;
    
    private Coroutine loopRoutine;

    [Header("Poof effect prefab")]
    public GameObject poofEffectPrefab;

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
    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            Blast();
            yield return new WaitForSeconds(1f / blastRate);
        }
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

    [ExposeMethod("Sets the temperature of the furnace")]
    public void SetTemperature(int heat)
    {
        this.heat = Math.Clamp(heat, 30, 250);
    }
    
    [ExposeMethod("Sets how many items this machine heats every second")]
    public void SetBlastRate(float rate)
    {
        blastRate = Mathf.Clamp(rate, 0.00001f, maxBlastRate);
    }

    public void Blast()
    {
        if (!sensor.detectedGameObject) return;
        var mcGibble = sensor.detectedGameObject.GetComponent<McGibble>();
        if (mcGibble == null) return;

        float stabilityClamped = Mathf.Clamp01(stability);
        float affinity = Mathf.Clamp01(mcGibble.description.normalizedHeatAffinity); // 0..1

        // Make affinity have a big effect: centered at 0.5 => 1, extremes go ±60%
        const float impact = 0.6f; // max deviation from 1 is ±impact
        float baseResistance = 1f + (affinity - 0.5f) * 2f * impact; // range [1 - impact, 1 + impact] => [0.4, 1.6]

        // Jitter damped by stability
        const float maxVariation = 0.15f;
        float noise = Random.Range(-1f, 1f) * maxVariation * (1f - stabilityClamped);

        float randomizedHeatResistance = Mathf.Max(0f, baseResistance + noise);
        float adjustedHeat = heat * randomizedHeatResistance;
        adjustedHeat = Mathf.Clamp(adjustedHeat, -100f, 500f);
        mcGibble.SetTemperature(Mathf.RoundToInt(adjustedHeat));

        if (poofEffectPrefab != null)
        {
            Vector3 offsetPosition = mcGibble.transform.position + new Vector3(0, 0, -1f);
            var poof = Instantiate(poofEffectPrefab, offsetPosition, Quaternion.identity);
        }

        tinyRandom = new Vector3(Random.value, Random.value - 0.5f, 0f);
        mcGibble.transform.position = outputPoint.transform.position + tinyRandom;
        if (poofEffectPrefab != null)
        {
            Vector3 offsetPosition = mcGibble.transform.position + new Vector3(0, 0, -1f);
            var bigPoof = Instantiate(poofEffectPrefab, offsetPosition, Quaternion.identity);
            bigPoof.transform.localScale *= 2f;
        }
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
