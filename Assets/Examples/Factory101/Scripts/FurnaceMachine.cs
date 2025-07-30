using System;
using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using Random = UnityEngine.Random;

public class FurnaceMachine : Machine, IPulseReceiver<McGibbleDescription>
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
    
    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            if(!string.IsNullOrWhiteSpace(script)){
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
    
    [ExposeMethod("Processes the item in the furnace")]
    public void Blast()
    {
        if (sensor.detectedGameObject)
        {
            //TODO Smart (per McGibble Type temperature and transmute settings)
            var mcGibble = sensor.detectedGameObject.GetComponent<McGibble>();
            mcGibble.description.heatResistance = heat;
            tinyRandom = new Vector3(Random.value, Random.value-0.5f, 0f);
            mcGibble.transform.position = outputPoint.transform.position + tinyRandom;
        }
    }

    public void OnPulse(McGibbleDescription mcGibble)
    {
        lastNotifiedMcGibble = mcGibble;
    }
}
