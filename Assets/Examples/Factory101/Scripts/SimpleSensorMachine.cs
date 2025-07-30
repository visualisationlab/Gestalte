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
    [SerializeField] private DraggableCableEnd cableEnd;

    private void Start()
    {
        RegisterLua();
    }

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            luaScript.DoString(script);
            yield return new WaitForSeconds(.5f);
        }
    }

    protected override void RegisterLua()
    {
        UserData.RegisterType<SimpleSensorMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    protected override void AfterSetScript()
    {
        StartCoroutine(ExecuteEverySecond());
    }

    [ExposeMethod("Detects objects in front of the machine")]
    public bool ReadSensor()
    {
        return sensor.onDetect;
    }

    [ExposeMethod("Returns the detected game object name")]
    public string GetDetectedObjectName()
    {
        return sensor.detectedGameObject.name;
    }

    [ExposeMethod("Emits a boolean signal out of the outport")]
    public void EmitOutPortSignal(bool signal)
    {
        cableEnd.SendPulse();
    }

    [ExposeMethod("Emits a string signal out of the outport")]
    public void EmitOutPortSignal(string signal)
    {
        cableEnd.SendPulse(signal);
    }
}