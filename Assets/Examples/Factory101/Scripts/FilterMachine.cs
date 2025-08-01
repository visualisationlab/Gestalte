using System.Collections;
using System.Collections.Generic;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using System.Linq;

public class FilterMachine : Machine
{
    public MultiSimpleSensor sensor;
    public Transform whitelistOutputPoint;
    public Transform blacklistOutputPoint;

    private Vector3 tinyRandom;
    protected HashSet<string> whitelist = new();
    protected bool whitelistSet = false;

    protected int minSalePrice = -1;
    protected bool priceFilterSet = false;


    private void Start()
    {
        StartCoroutine(ExecuteEverySecond());
    }

    protected override void RegisterLua()
    {
        UserData.RegisterType<FilterMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    public override string GetStatus()
    {
        return "Filter Status";
    }

    public override string UpgradeMachine()
    {
        throw new System.NotImplementedException();
    }

    [ExposeMethod("setMinSalePrice")]
    public void SetMinSalePrice(int value)
    {
        priceFilterSet = true;
        minSalePrice = value;
    }

    [ExposeMethod("setWhitelistGibbles")]
    public void SetWhitelistGibbles(Table luaTable)
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

    [ExposeMethod("Filter the item in the filter machine")]
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

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            if (!string.IsNullOrWhiteSpace(script))
            {
                luaScript.DoString(script);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}