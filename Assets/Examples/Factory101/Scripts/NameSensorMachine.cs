using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class NameSensorMachine : Machine
{
    [SerializeField] private NameSensor sensor;
    [SerializeField] private OracleAgent oracle;
    private void Start()
    {
       RegisterLua();
    }

    protected override void RegisterLua()
    {
        UserData.RegisterType<NameSensorMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    protected override void AfterSetScript()
    {
        luaScript.DoString(script);
    }

    [ExposeMethod("Tells you the name of the object in front of the machine. You can add a cool instruction.")]
    public void ReadSensor(string instruction)
    {
        Debug.Log(instruction);
        Debug.Log(sensor.nameDetected);
        oracle.SendMessage(instruction + ": "+sensor.nameDetected, OnResponseTranslation);
    }

    public void OnResponseTranslation(string response)
    {
        Debug.Log(response);
    }
}
