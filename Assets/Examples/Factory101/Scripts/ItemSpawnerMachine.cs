using System.Collections;
using UnityEngine;

public class ItemSpawnerMachine : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float tickRate;
    private Vector3 tinyRandom;
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
        tinyRandom = new Vector3(Random.value, Random.value-0.5f, 0f);
        var mcGibble = Instantiate(prefab, spawnPoint.transform.position + tinyRandom, Quaternion.identity).GetComponent<McGibble>();
        McGibbleTracker.Instance.Add(mcGibble);
    }
    
}
