using System.Collections;
using System.Collections.Generic;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Events;

public class SimpleSensorMachine : Machine
{
    public SimpleSensor sensor;
    private Script luaScript;
    private string script;
    public UnityEvent<bool> Outport;

    private void Start()
    {
        UserData.RegisterType<SimpleSensorMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            luaScript.DoString(script);
            yield return new WaitForSeconds(.5f);
        }
    }

    public override void SetScript(string code)
    {
        script = code;
        StartCoroutine(ExecuteEverySecond());
    }
    
    [ExposeMethod("Detects objects in front of the machine")]
    public bool ReadSensor()
    {
        return sensor.onDetect;
    }
    
    [ExposeMethod("Emits a boolean signal out of the outport")]
    public void EmitOutPortSignal(bool signal)
    {
        Outport.Invoke(signal);
    }
}