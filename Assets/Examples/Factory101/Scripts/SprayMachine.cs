using System;
using System.Collections;
using Examples.Factory101.Scripts;
using Examples.Factory101.Scripts.Input;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.Events;
using Coroutine = UnityEngine.Coroutine;

public class SprayMachine : Machine, IBuyable, IBlockPlacement, IPulseReceiver<McGibbleDescription>
{
    [Header("Detection & Output")]
    public SimpleSensor sensor;
    public Transform outputPoint;

    [Header("Spray Settings")]
    public Color sprayColor = Color.magenta;
    public FinishType finishType = FinishType.Mat;
    [Range(0f, 1f)] public float intensity = 1f; // 0 = no change, 1 = full spray
    
    [SerializeField] private float sprayRate;
    private float maxSprayRate = 1f;
    private Coroutine loopRoutine;

    [Header("Poof effect prefab")]
    public GameObject poofEffectPrefab;

    public McGibbleDescription lastMcGibbleDetected;
    
    protected override void RegisterLua()
    {
        UserData.RegisterType<SprayMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
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
        result += $"Rate: {GetSprayRate()} item(s)/s (max {GetMaxSprayRate()})\n";
        result += $"Finish Type: {finishType.ToString()}\n";
        return result;
    }

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            Spray();
            yield return new WaitForSeconds(1f / sprayRate);
        }
    }
    
    public override void UpgradeMachine()
    {
        upgradeLevel++;
        
        maxSprayRate = upgradeLevel switch
        {
            2 => 2f,
            3 => 3f,
            4 => 4f,
            5 => 5f,
            6 => 6f,
            _ => 1f // default case
        };

        sprayRate = maxSprayRate;

        finishType = upgradeLevel switch
        {
            2 => FinishType.Glossy,
            3 => FinishType.Shiny,
            4 => FinishType.Metallic,
            5 => FinishType.Pearlescent,
            6 => FinishType.Galactic,
            _ => FinishType.Mat // default case
        };
    }

    [ExposeMethod("Sets the spray color (r,g,b) in 0.0 - 1.0 range")]
    public void SetSprayColor(float r, float g, float b)
    {
        sprayColor = new Color(r, g, b);
    }
    
    [ExposeMethod("Sets how many items this machine sprays every second")]
    public void SetSprayRate(float rate)
    {
        sprayRate = Mathf.Clamp(rate, 0.000001f, maxSprayRate);
    }
    
    [ExposeMethod("See if there is an item detected")]
    public bool IsItemDetected()
    {
        return lastMcGibbleDetected != null;
    }
    
    [ExposeMethod("Get the name of the item the connected sensor detected")]
    public string GetNameOfDetectedItem()
    {
        return lastMcGibbleDetected == null ? "" : lastMcGibbleDetected.name;
    }
    
    [ExposeMethod("Get the base sale price of the item the connected sensor detected")]
    public int GetBasePriceOfDetectedItem()
    {
        return lastMcGibbleDetected == null ? 0 : lastMcGibbleDetected.raritySalePrice;
    }
    
    [ExposeMethod("Get the normalized rarity of the item the connected sensor detected")]
    public float GetRarityOfDetectedItem()
    {
        return lastMcGibbleDetected == null ? 0.0f : lastMcGibbleDetected.normalizedRarity;
    }

    public void Spray()
    {
        if (!sensor.detectedGameObject) return;
        var mcGibble = sensor.detectedGameObject.GetComponent<McGibble>();
        if (mcGibble == null) return;

        if (poofEffectPrefab != null)
        {
            Vector3 offsetPosition = mcGibble.transform.position + new Vector3(0, 0, -1f);
            var poof = Instantiate(poofEffectPrefab, offsetPosition, Quaternion.identity);
        }
        mcGibble.transform.position = outputPoint.position + new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0f);
        mcGibble.SetSprayPaint(finishType, sprayColor);
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
    
    private string GetSprayRate()
    {
        return sprayRate.ToString("0.0");
    }

    private string GetMaxSprayRate()
    {
        return maxSprayRate.ToString("0.0");
    }

    public void OnPulse(McGibbleDescription message)
    {
        lastMcGibbleDetected = message;
        ExecuteScript();
    }
}
