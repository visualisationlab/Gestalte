using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class NameSensorMachine : Machine
{
    private Script luaScript;
    private string script;
    [SerializeField] private NameSensor sensor;
    [SerializeField] private OracleAgent oracle;
    private void Start()
    {
        UserData.RegisterType<NameSensorMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    public override void SetScript(string code)
    {
        script = code;
        Debug.Log($"Executing: {code}");
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
