using System.Collections;
using UnityEngine;

public class ItemSpawnerMachine : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float tickRate;
    private Vector3 tinyRandom;
    public ConveyorBelt neighbourConveyor;
    public void Start()
    {
        StartCoroutine(ExecuteEverySecond());
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
        if(neighbourConveyor && neighbourConveyor.HasSpace(0f)){
            tinyRandom = new Vector3(Random.value, Random.value-0.2f, 0f);
            var instance = Instantiate(prefab, spawnPoint.transform.position + tinyRandom, Quaternion.identity);
            McGibbleTracker.Add(instance);
            neighbourConveyor.TakeOverItem(instance, 0f);
        }
    }
    
}
