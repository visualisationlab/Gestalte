using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class McGibbleTracker : MonoBehaviour
{
    public static McGibbleTracker Instance { get; private set; }

    [SerializeField]
    private float expireTime = 10f;

    private class TrackedMcGibble
    {
        public McGibble McGibble;
        public float ExpiryTime;
    }

    private readonly List<TrackedMcGibble> trackedObjects = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate McGibbleTracker detected. Destroying new instance.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(CleanupTick());
    }

    IEnumerator CleanupTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CleanupExpired();
        }
    }

    public void Add(McGibble obj)
    {
        trackedObjects.Add(new TrackedMcGibble
        {
            McGibble = obj,
            ExpiryTime = Time.time + expireTime
        });
    }

    public void Remove(McGibble obj)
    {
        trackedObjects.RemoveAll(to => to.McGibble == obj);
        Destroy(obj.gameObject);
    }

    public IEnumerable<McGibble> GetAll()
    {
        return trackedObjects.Select(to => to.McGibble);
    }

    public int Count => trackedObjects.Count;

    private void CleanupExpired()
    {
        float now = Time.time;

        for (int i = trackedObjects.Count - 1; i >= 0; i--)
        {
            if (trackedObjects[i].ExpiryTime <= now)
            {
                Remove(trackedObjects[i].McGibble);
            }
        }
    }
}
