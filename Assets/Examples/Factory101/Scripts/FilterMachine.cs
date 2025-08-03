using System.Collections;
using System.Collections.Generic;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using System.Linq;
using Examples.Factory101.Scripts.Input;
using Coroutine = UnityEngine.Coroutine;

public class FilterMachine : Machine, IBuyable, IBlockPlacement
{
    public MultiSimpleSensor sensor;
    public Transform whitelistOutputPoint;
    public Transform blacklistOutputPoint;

    private Vector3 tinyRandom;
    protected HashSet<string> whitelist = new();
    protected bool whitelistSet = false;

    protected int minSalePrice = -1;
    protected bool priceFilterSet = false;

    [SerializeField] private float filterRate;
    [SerializeField] private float maxFilterRate;

    private Coroutine loopRoutine;
    
    protected override void RegisterLua()
    {
        UserData.RegisterType<FilterMachine>();
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
        result += $"Rate: {GetFilterRate()} item(s)/s (max {GetMaxFilterRate()})\n";
        return result;
    }
    
    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            Filter();
            yield return new WaitForSeconds(1f / filterRate);
        }
    }
    
    public override void UpgradeMachine()
    {
        upgradeLevel++;
        
        maxFilterRate = upgradeLevel switch
        {
            2 => 3f,
            3 => 5f,
            4 => 6f,
            _ => 1f // default case
        };
    }
    
    [ExposeMethod("Sets the rate this machine filters items with every second")]
    public void SetFilterRate(float rate)
    {
        filterRate = Mathf.Clamp(rate, 0.00001f, maxFilterRate);
    }

    [ExposeMethod("Filter on minimal Sale Price")]
    public void SetMinSalePrice(int value)
    {
        priceFilterSet = true;
        minSalePrice = value;
    }

    [ExposeMethod("Filter on name")]
    public void SetFilterOnName(Table luaTable)
    {
        whitelistSet = true;
        whitelist.Clear();
        foreach (var kv in luaTable.Values)
        {
            whitelist.Add(kv.String.ToLower());
        }
    }

    public virtual bool IsAllowed(GameObject obj)
    {
        var gibble = obj.GetComponent<McGibble>();
        if (gibble == null) return false;

        if (priceFilterSet && gibble.description.raritySalePrice < minSalePrice)
            return false;

        if (whitelistSet)
            return whitelist.Contains(gibble.description.singleEmoji.ToLower());

        // If at least one filter is active and passed, allow
        if (priceFilterSet || whitelistSet)
            return true;

        return false; // deny all if nothing defined
    }

    public void Filter()
    {
        foreach (var obj in sensor.detectedGameObjects.ToList())
        {
            if (obj == null) continue;
            var gibble = obj.GetComponent<McGibble>();
            if (gibble == null) continue;

            tinyRandom = new Vector3(Random.value * 0.2f - 0.1f, Random.value * 0.2f - 0.1f, 0f);

            var basePos = IsAllowed(obj) ? whitelistOutputPoint.position : blacklistOutputPoint.position;
            obj.transform.position = basePos + tinyRandom;
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
    
    private string GetFilterRate()
    {
        return filterRate.ToString("0.0");
    }

    private string GetMaxFilterRate()
    {
        return maxFilterRate.ToString("0.0");
    }

}