using UnityEngine;
using Mediator;

public class LightMachine : MonoBehaviour, IPulseReceiver
{
    [SerializeField] private SpriteRenderer lightImage;

    private void Start()
    {
        SetState(false);
    }

    public void SetState(bool state)
    {
        lightImage.color = state ? Color.yellow : Color.grey;
    }

    public void OnPulse()
    {
        Debug.Log("Receiver activated by pulse!");
        // Do your logic here
        SetState(true);
    }

    public void OnPulse(string message)
    {
        // Do nothing
        Debug.Log($"Receiver activated by pulse with message: {message}");
    }
}
