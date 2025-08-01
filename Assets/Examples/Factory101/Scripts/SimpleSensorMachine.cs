using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class SimpleSensorMachine : Machine, IBuyable
{
    public SimpleSensor sensor;
    [SerializeField] private DraggableCableEnd cableEnd;

    public int GetPrice()
    {
        return basePrice;
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
        UserData.RegisterType<McGibbleDescription>(InteropAccessMode.Default);
        luaScript = new Script();
        luaScript.Globals["McGibbleDescription"] = UserData.CreateStatic<McGibbleDescription>();
        luaScript.Globals["this"] = this;
    }

    public override string GetStatus()
    {
        return "Get Simple sensor Status";
    }

    public override void UpgradeMachine()
    {
        throw new System.NotImplementedException();
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
    
    public string GetDescription()
    {
        return description;
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