using System;
using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using Random = UnityEngine.Random;

public class FurnaceMachine : Machine
{
    public SimpleSensor sensor;
    public Transform outputPoint;
    public GameObject prefab;
    private int heat = 100;
    private Vector3 tinyRandom;

    private void Start()
    {
        RegisterLua();
        StartCoroutine(ExecuteEverySecond());
    }

    protected override void RegisterLua()
    {
        UserData.RegisterType<FurnaceMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
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
    
    [ExposeMethod("Processes the item in the furnace")]
    public void Blast()
    {
        if (sensor.detectedGameObject)
        {
            //TODO Smart (per McGibble Type temperature and transmute settings)
            var mcGibble = sensor.detectedGameObject.GetComponent<McGibble>();
            mcGibble.description.heat = heat;
            tinyRandom = new Vector3(Random.value, Random.value-0.5f, 0f);
            mcGibble.transform.position = outputPoint.transform.position + tinyRandom;
        }
    }
}
