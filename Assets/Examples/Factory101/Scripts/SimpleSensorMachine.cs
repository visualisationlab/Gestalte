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
        UserData.RegisterType<McGibbleDescription>(InteropAccessMode.Default);
        luaScript = new Script();
        luaScript.Globals["McGibbleDescription"] = UserData.CreateStatic<McGibbleDescription>();
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
    
    [ExposeMethod("Returns the detected game objects description")]
    public McGibbleDescription GetDescription()
    {
        return sensor.detectedGameObject.GetComponent<McGibble>().description;
    }

    [ExposeMethod("Emits a boolean signal out of the outport")]
    public void EmitOutPortSignal(bool signal)
    {
        cableEnd.SendPulse();
    }

    [ExposeMethod("Emits the description over the output port")]
    public void EmitDescription(McGibbleDescription signal)
    {
        cableEnd.SendPulse(signal);
    }
}