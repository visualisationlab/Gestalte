using System.Collections;
using Examples.Factory101.Scripts;
using Examples.Factory101.Scripts.Input;
using MoonSharp.Interpreter;
using UnityEngine;
using Coroutine = UnityEngine.Coroutine;

public class SimpleSensorMachine : Machine, IBuyable, IBlockPlacement
{
    public SimpleSensor sensor;
    [SerializeField] private DraggableCableEnd cableEnd;
    
    private Coroutine loopRoutine;

    protected override void RegisterLua()
    {
        UserData.RegisterType<SimpleSensorMachine>();
        UserData.RegisterType<McGibbleDescription>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
        luaScript.Globals["McGibbleDescription"] = UserData.CreateStatic<McGibbleDescription>();
    }

    public override void UpgradeMachine()
    {
        upgradeLevel++;
    }

    protected override void AfterSetScript()
    {
        ExecuteScript();
    }
    
    public override string GetStatus()
    {
        string result = "";
        result += $"Level: {upgradeLevel}/{maxUpgradeLevel} \n";
        return result;
    }
    public void Sense(GameObject detectedGameObject)
    {
        var mcGibble = detectedGameObject.GetComponent<McGibble>();
        if (mcGibble)
        {
            cableEnd.SendPulse(mcGibble.description);
            Debug.Log($"SENSOR DETECTED: {mcGibble.description.name}");
        }
    }
    
    public int GetPrice()
    {
        return basePrice;
    }

    public string GetDescription()
    {
        return description;
    }
    
}