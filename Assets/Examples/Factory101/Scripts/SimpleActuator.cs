using System;
using System.Collections;
using Examples.Factory101.Scripts;
using Examples.Factory101.Scripts.Input;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using Coroutine = UnityEngine.Coroutine;

public class SimpleActuator : Machine, IBuyable, IBlockPlacement
{
    public ActuatorPiston piston;

    [SerializeField] public float pushRate = .5f;
    [SerializeField] public float maxPushRate = 1;//once every second
    
    private Coroutine loopRoutine;
    
    protected override void RegisterLua()
    {
        UserData.RegisterType<SimpleActuator>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
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
        result += $"Push rate: {pushRate}/{maxPushRate} \n";
        return result;
    }
    
    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            Push();
            yield return new WaitForSeconds(1f / pushRate);
            Retract();
            yield return new WaitForSeconds(1f / pushRate);
        }
    }
    
    public override void UpgradeMachine()
    {
        upgradeLevel++;
        
        maxPushRate = upgradeLevel switch
        {
            2 => 2f,
            3 => 3f,
            _ => 1f // default case
        };
    }
    
    public int GetPrice()
    {
        return basePrice;
    }

    public string GetDescription()
    {
        return description;
    }
    
    [ExposeMethod("Sets the rate the machine pushes with")]
    public void SetPushRate(int rate)
    {
        pushRate = Math.Clamp(rate, 0.000001f, maxPushRate);
    }

    public void Push()
    {
        piston.Extend();
    }
    
    public void Retract()
    {
        piston.Retract();
    }
    
}
