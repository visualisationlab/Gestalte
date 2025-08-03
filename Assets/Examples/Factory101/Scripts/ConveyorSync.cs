using UnityEngine;

public class ConveyorSync : MonoBehaviour
{
    // Global reference time when belts start moving. Can be reset to restart sync.
    public static float GlobalStartTime { get; private set; }
    public static float GlobalSpeed = 1f; // multiplier if you want to speed up / slow down all belts

    void Awake()
    {
        GlobalStartTime = Time.time;
    }

    // Call this if you want to restart all belts in-phase from now:
    public static void RestartSync()
    {
        GlobalStartTime = Time.time;
    }
}
