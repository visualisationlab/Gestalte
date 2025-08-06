using System.Collections;
using System.Collections.Generic;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using System.Linq;
using Examples.Factory101.Scripts.Input;
using Coroutine = UnityEngine.Coroutine;

public class DroneMachine : Machine, IBuyable, IBlockPlacement
{
    public MultiSimpleSensor grabArea;
    public Transform dropAreaCenter;
    public float dropRadius = 1.0f;
    protected HashSet<string> whitelist = new();
    protected bool whitelistSet = false;

    protected int minSalePrice = -1;
    protected bool priceFilterSet = false;
    protected float minHeatResistance = -1;
    protected bool heatResistanceFilterSet = false;
    public GameObject dronePrefab;
    public int poolSize = 10;
    private List<DroneWorker> dronePool = new();
    private Queue<DroneWorker> availableDrones = new();
    private HashSet<GameObject> claimedGibbles = new();

    private Coroutine loopRoutine;

    [SerializeField] private float droneRate;
    [SerializeField] private float maxDroneRate;


    protected override void RegisterLua()
    {
        UserData.RegisterType<DroneMachine>();
        UserData.RegisterType<McGibbleDescription>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
        luaScript.Globals["McGibbleDescription"] = UserData.CreateStatic<McGibbleDescription>();
        loopRoutine = StartCoroutine(ExecuteEverySecond());
        InstantiateDrones();
    }

    private void InstantiateDrones()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var drone = Instantiate(dronePrefab, transform.position, Quaternion.identity);
            var worker = drone.GetComponent<DroneWorker>();
            drone.SetActive(false);
            dronePool.Add(worker);
            availableDrones.Enqueue(worker);
        }
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
        result += $"Rate: {GetDroneRate()} item(s)/s (max {GetMaxDroneRate()})\n";
        return result;
    }


    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            MoveGibbles();
            yield return new WaitForSeconds(1f / droneRate);
        }
    }

    public override void UpgradeMachine()
    {
        upgradeLevel++;
        
        maxDroneRate = upgradeLevel switch
        {
            2 => 3f,
            3 => 5f,
            4 => 6f,
            _ => 1f // default case
        };
    }

    [ExposeMethod("Sets the rate this machine sends out drones")]
    public void SetDroneRate(float rate)
    {
        droneRate = Mathf.Clamp(rate, 0.00001f, maxDroneRate);
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

    [ExposeMethod("Set filter on heatresistance")]
    public void SetFilterOnHeatResistanceNormalized(float normalizedValue)
    {
        heatResistanceFilterSet = true;
        minHeatResistance = normalizedValue;
    }

    public virtual bool IsAllowed(GameObject obj)
    {
        var gibble = obj.GetComponent<McGibble>();
        if (gibble == null) return false;

        if (priceFilterSet && gibble.description.raritySalePrice < minSalePrice)
            return false;

        if (heatResistanceFilterSet && gibble.description.normalizedHeatAffinity < minHeatResistance)
            return false;

        if (whitelistSet)
            return whitelist.Contains(gibble.description.name.ToLower());

        // If at least one filter is active and passed, allow
        if (priceFilterSet || whitelistSet || heatResistanceFilterSet)
            return true;

        return false; // deny all if nothing defined
    }

    public void MoveGibbles()
    {
        foreach (var obj in grabArea.detectedGameObjects.ToList())
        {
            if (obj == null || !IsAllowed(obj)) continue;
            if (claimedGibbles.Contains(obj)) continue;
            if (availableDrones.Count == 0) break;
            
            claimedGibbles.Add(obj); // ✅ Mark as in use

            var drone = availableDrones.Dequeue();
            drone.gameObject.SetActive(true);
            drone.transform.position = transform.position;

            drone.StartJob(obj, dropAreaCenter, (returnedDrone) =>
            {
                claimedGibbles.Remove(obj); // still safe even if obj was destroyed
                returnedDrone.gameObject.SetActive(false);
                availableDrones.Enqueue(returnedDrone);
            });
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
    
    private string GetDroneRate()
    {
        return droneRate.ToString("0.0");
    }

    private string GetMaxDroneRate()
    {
        return maxDroneRate.ToString("0.0");
    }

}