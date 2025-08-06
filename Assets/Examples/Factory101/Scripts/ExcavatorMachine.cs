using System;
using System.Collections;
using Examples.Factory101.Scripts;
using Examples.Factory101.Scripts.Input;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using Coroutine = UnityEngine.Coroutine;
using Random = UnityEngine.Random;

public class ExcavatorMachine : Machine, IBuyable, IBlockPlacement
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject mcGibbleTemplate;
    [SerializeField] private RecipeScriptableObject startRecipe;
    
    private Coroutine loopRoutine;
    
    //Dig rate
    [SerializeField] private float digRate;
    private float maxDigRate = 0.5f;

    //Placement Spread
    private Vector3 spread;
    private float placementSpread = 0.5f;
    private float minPlacementSpread = 0.1f;
    private float maxPlacementSpread = 0.5f;

    public int GetPrice()
    {
        return basePrice;
    }

    public string GetDescription()
    {
        return description;
    }

    public void Start()
    {
        loopRoutine = StartCoroutine(ExecuteEverySecond());
    }

    protected override void RegisterLua()
    {
        UserData.RegisterType<ExcavatorMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    public override string GetStatus()
    {
        string result = "";
        result += $"Level: {upgradeLevel}/{maxUpgradeLevel} \n";
        result += $"Rate: {GetDigRate()} item(s)/s (max {GetMaxDigRate()})\n";
        result += $"Spread: {placementSpread} (min {minPlacementSpread}, max {maxPlacementSpread})\n";
        return result;
    }

    public override void UpgradeMachine()
    {
        upgradeLevel++;
        maxDigRate = upgradeLevel switch
        {
            2 => 2f,
            3 => 3f,
            _ => 1f // default case
        };
        
        digRate = maxDigRate;

        maxPlacementSpread = upgradeLevel switch
        {
            2 => 0.75f,
            3 => 1f,
            _ => .5f // default case
        };
    }

    protected override void AfterSetScript()
    {
        ExecuteScript();
        RestartCoroutine();
    }

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(1f / digRate);
        }
    }

    [ExposeMethod("Sets how many items this machine digs up per second.")]
    public void SetDigRate(float rate)
    {
        digRate = Mathf.Clamp(rate, 00000.1f, maxDigRate);
    }
    
    [ExposeMethod("Sets the size of the placement area.")]
    public void SetPlacementSpread(float newSpread)
    {
        placementSpread = Mathf.Clamp(newSpread, minPlacementSpread, maxPlacementSpread);
    }

    private void SpawnItem()
    {
        spread = new Vector3(Random.value, Random.value - placementSpread, 0f);
        var mcGibble =
            Instantiate(mcGibbleTemplate, spawnPoint.transform.position + spread, Quaternion.identity)
                .GetComponent<McGibble>();
        mcGibble.description = startRecipe.recipe.result;
        McGibbleTracker.Instance.Add(mcGibble);
    }

    private string GetDigRate()
    {
        return digRate.ToString("0.0");
    }

    private string GetMaxDigRate()
    {
        return maxDigRate.ToString("0.0");
    }
    
    private void RestartCoroutine()
    {
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
        }

        loopRoutine = StartCoroutine(ExecuteEverySecond());
    }

}