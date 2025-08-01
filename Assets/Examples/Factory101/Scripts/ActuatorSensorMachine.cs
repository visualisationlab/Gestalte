using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class ActuatorSensorMachine : Machine
{
    public SimpleSensor sensor;
    public ActuatorPiston piston;
    private void Start()
    {
        StartCoroutine(ExecuteEverySecond());
    }

    protected override void RegisterLua()
    {
        UserData.RegisterType<ActuatorSensorMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    public override string GetStatus()
    {
        return "Get Actuator Sensor Status";
    }

    public override void UpgradeMachine()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            ExecuteScript();
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
