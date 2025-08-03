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
    [SerializeField] private float senseRate;
    [SerializeField] private float maxSenseRate = 1f;
    
    private Coroutine loopRoutine;

    protected override void RegisterLua()
    {
        UserData.RegisterType<SimpleSensorMachine>();
        UserData.RegisterType<McGibbleDescription>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
        luaScript.Globals["McGibbleDescription"] = UserData.CreateStatic<McGibbleDescription>();
        loopRoutine = StartCoroutine(ExecuteEverySecond());
    }
    
    protected override void AfterSetScript()
    {
        ExecuteScript();
        RestartCoroutine();
    }
    
    private void RestartCoroutine()
    {
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
        }

        loopRoutine = StartCoroutine(ExecuteEverySecond());
    }
    public override string GetStatus()
    {
        string result = "";
        result += $"Level: {upgradeLevel}/{maxUpgradeLevel} \n";
        result += $"Rate: {GetSenseRate()} item(s)/s (max {GetMaxSenseRate()})\n";
        return result;
    }
    
    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            Sense();
            yield return new WaitForSeconds(1f / senseRate);
        }
    }
    
    public override void UpgradeMachine()
    {
        upgradeLevel++;
        
        maxSenseRate = upgradeLevel switch
        {
            2 => 3f,
            3 => 5f,
            4 => 6f,
            _ => 1f // default case
        };
        
    }

    public void Sense()
    {
        if (sensor.onDetect)
        {
            var mcGibble = sensor.detectedGameObject.GetComponent<McGibble>();
            if (mcGibble)
            {
                cableEnd.SendPulse(mcGibble.description);
                Debug.Log($"SENSOR DETECTED: {mcGibble.description.name}");
            }
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
    
    private string GetSenseRate()
    {
        return senseRate.ToString("0.0");
    }

    private string GetMaxSenseRate()
    {
        return maxSenseRate.ToString("0.0");
    }
}