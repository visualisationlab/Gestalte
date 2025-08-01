using System;
using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using Random = UnityEngine.Random;

public class FurnaceMachine : Machine, IPulseReceiver<McGibbleDescription>, IPulseReceiver<bool>, IBuyable
{
    public SimpleSensor sensor;
    public Transform outputPoint;
    public int heat = 100;
    private Vector3 tinyRandom;
    private McGibbleDescription lastNotifiedMcGibble;

    private void Start()
    {
        StartCoroutine(ExecuteEverySecond());
    }

    protected override void RegisterLua()
    {
        UserData.RegisterType<FurnaceMachine>();
        UserData.RegisterType<McGibbleDescription>(InteropAccessMode.Default);
        luaScript = new Script();
        luaScript.Globals["this"] = this;
        luaScript.Globals["McGibbleDescription"] = UserData.CreateStatic<McGibbleDescription>();
    }

    public override string GetStatus()
    {
        return "Furnace Status";
    }

    public override void UpgradeMachine()
    {
        throw new NotImplementedException();
    }

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            if (!string.IsNullOrWhiteSpace(script)) {
                luaScript.DoString(script);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    [ExposeMethod("Sets the heat of the furnace")]
    public void SetHeat(int heat)
    {
        this.heat = heat;
    }

    [ExposeMethod("Get the normalizedRarity of the last notified mcGibble")]
    public float McGibbleRarity()
    {
        if (lastNotifiedMcGibble != null)
        {
            return lastNotifiedMcGibble.normalizedRarity;
        }

        return 0.0f;
    }

    [ExposeMethod("Get the normalized heat resistance of last notified McGibbleDescription")]
    public float McGibbleHeatResistance()
    {
        if (lastNotifiedMcGibble != null)
        {
            return lastNotifiedMcGibble.normalizedHeatResistance;
        }

        return 0.0f;
    }

    [ExposeMethod("Processes the item in the furnace")]
    public void Blast()
    {
        if (sensor.detectedGameObject)
        {
            //TODO Smart (per McGibble Type temperature and transmute settings)
            var mcGibble = sensor.detectedGameObject.GetComponent<McGibble>();
            var heatResistance = mcGibble.description.normalizedHeatResistance;

            float adjustedHeat = heat * heatResistance;
            mcGibble.heat = (int)Math.Round(adjustedHeat);

            tinyRandom = new Vector3(Random.value, Random.value - 0.5f, 0f);
            mcGibble.transform.position = outputPoint.transform.position + tinyRandom;
        }
    }


    [ContextMenu("Test Blast")]
    public void testBlast()
    {
        Blast();
    }

    [ContextMenu("Test Set Heat")]
    public void testSetHeat()
    {
        SetHeat(100);
    }
    public void OnPulse(McGibbleDescription mcGibble)
    {
        lastNotifiedMcGibble = mcGibble;
    }
    
    public void OnPulse(bool pulse)
    {
        if (pulse)
        {
            Blast();
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
}
