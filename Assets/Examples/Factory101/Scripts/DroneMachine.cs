using System.Collections;
using System.Collections.Generic;
using Examples.Factory101.Scripts;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;
using System.Linq;
using Examples.Factory101.Scripts.Input;

public class DroneMachine : Machine, IBuyable, IBlockPlacement
{
    public MultiSimpleSensor grabArea;
    public Transform dropAreaCenter;
    public float dropRadius = 1.0f;
    protected HashSet<string> whitelist = new();
    protected bool whitelistSet = false;

    protected int minSalePrice = -1;
    protected bool priceFilterSet = false;
    public GameObject dronePrefab;
    public int poolSize = 10;
    private List<DroneWorker> dronePool = new();
    private Queue<DroneWorker> availableDrones = new();
    private HashSet<GameObject> claimedGibbles = new();

    public int GetPrice()
    {
        return basePrice;
    }

    public string GetDescription()
    {
        return description;
    }

    private void Start()
    {
        StartCoroutine(ExecuteEverySecond());

        for (int i = 0; i < poolSize; i++)
        {
            var drone = Instantiate(dronePrefab, transform.position, Quaternion.identity);
            var worker = drone.GetComponent<DroneWorker>();
            drone.SetActive(false);
            dronePool.Add(worker);
            availableDrones.Enqueue(worker);
        }
    }
    
    protected override void RegisterLua()
    {
        UserData.RegisterType<DroneMachine>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    public override string GetStatus()
    {
        return "Get Drone Status";
    }

    public override void UpgradeMachine()
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




    IEnumerator ExecuteEverySecond()
    {
        while (true)
        {
            ExecuteScript();
            yield return new WaitForSeconds(1f);
        }
    }

    [ExposeMethod("Move Gibbles using drones")]
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


}