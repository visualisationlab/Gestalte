using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class FurnaceMachine : Machine
{
    public SimpleSensor sensor;
    public Transform outputPoint;
    public GameObject prefab;
    private Script luaScript;
    private string script;
    private int heat = 100;
    private Vector3 tinyRandom;
    private void Start()
    {
        UserData.RegisterType<FurnaceMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
        StartCoroutine(ExecuteEverySecond());
    }

    public override void SetScript(string code)
    {
        script = code;
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
            McGibbleTracker.Remove(sensor.detectedGameObject);
            Destroy(sensor.detectedGameObject);
            tinyRandom = new Vector3(Random.value, Random.value-0.5f, 0f);
            var instance = Instantiate(prefab, outputPoint.transform.position + tinyRandom, Quaternion.identity);
            McGibbleTracker.Add(instance);
        }
    }
}
