using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class SimpleActuator : Machine
{
    public ActuatorPiston piston;
    private Script luaScript;
    private string script;
    
    private void Start()
    {
        UserData.RegisterType<SimpleActuator>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }
    
    public override void SetScript(string code)
    {
        script = code;
        StartCoroutine(ExecuteEverySecond());
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
