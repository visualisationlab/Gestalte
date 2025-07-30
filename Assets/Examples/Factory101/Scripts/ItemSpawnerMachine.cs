using System.Collections;
using Examples.Factory101.Scripts;
using MoonSharp.Interpreter;
using UnityEngine;

public class ItemSpawnerMachine : Machine
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject prefab;
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
    
    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(tickRate);
        }
    }

    private void SpawnItem()
    {
        tinyRandom = new Vector3(Random.value, Random.value-0.5f, 0f);
        var mcGibble = Instantiate(prefab, spawnPoint.transform.position + tinyRandom, Quaternion.identity).GetComponent<McGibble>();
        McGibbleTracker.Instance.Add(mcGibble);
    }

   
}
