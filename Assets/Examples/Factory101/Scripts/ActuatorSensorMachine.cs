using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class ActuatorSensorMachine : Machine
{
    public SimpleSensor sensor;
    public ActuatorPiston piston;
    private Script luaScript;
    private string script;
    private void Start()
    {
        UserData.RegisterType<ActuatorSensorMachine>();
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
            if (!string.IsNullOrWhiteSpace(script))
            {
                luaScript.DoString(script);
            }
            yield return new WaitForSeconds(.5f);
        }
    }
    
    [ExposeMethod("Detects objects in front of the machine")]
    public bool ReadSensor()
    {
        return sensor.onDetect;
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
