using System.Collections;
using System.Collections.Generic;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class ActuatorMachine : MonoBehaviour
{
    public ActuatorSensor sensor;
    public ActuatorPiston piston;
    private Script luaScript;
    private string script;
    private void Start()
    {
        UserData.RegisterType<ActuatorMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    public void SetScript(string code)
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
    
    [ExposeMethod("Detects objects in front of the machine")]
    public bool ReadSensor()
    {
        Debug.Log("Sensor Read");
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
