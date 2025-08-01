using System;
using System.Collections;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawnerMachine : Machine
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject mcGibbleTemplate;
    [SerializeField] private RecipeScriptableObject startRecipe;
    [SerializeField] private int tickRate;
    private Vector3 tinyRandom;
    
    public void Start()
    {
        StartCoroutine(ExecuteEverySecond());
    }
    
    protected override void RegisterLua()
    {
        UserData.RegisterType<ItemSpawnerMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    public override string GetStatus()
    {
        return "Item Spawner Status";
    }

    public override string UpgradeMachine()
    {
        throw new NotImplementedException();
    }

    protected override void AfterSetScript()
    {
        luaScript.DoString(script);
    }

    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(tickRate);
        }
    }

    [ExposeMethod("Sets the rate this machine spawns items at")]
    public void SetSpawnRate(int rate)
    {
        tickRate = rate;
        if (tickRate == 0) tickRate = Int32.MaxValue;
    }

    private void SpawnItem()
    {
        tinyRandom = new Vector3(Random.value, Random.value-0.5f, 0f);
        var mcGibble = Instantiate(mcGibbleTemplate, spawnPoint.transform.position + tinyRandom, Quaternion.identity).GetComponent<McGibble>();
        mcGibble.description = startRecipe.recipe.result;
        McGibbleTracker.Instance.Add(mcGibble);
    }
   
}
