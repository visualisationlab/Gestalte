using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class SimpleActuator : Machine
{
    public ActuatorPiston piston;

    public override string GetStatus()
    {
        return "Get Actuator Status";
    }

    protected override void AfterSetScript()
    {
        StartCoroutine(ExecuteEverySecond());
    }

    protected override void RegisterLua()
    {
        UserData.RegisterType<SimpleActuator>();
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
    
    [ExposeMethod("Pushes actuator piston out")]
    public void Push()
    {
        piston.Extend();
    }
    
    [ExposeMethod("Retracts actuator piston")]
    public void Retract()
    {
        piston.Retract();
    }
    
}
