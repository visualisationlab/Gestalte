using UnityEngine;
using Mediator;

public class StringMachine : MonoBehaviour, IPulseReceiver
{
    [SerializeField] private string message = "Default Message";
    public void OnPulse(string message)
    {
        Debug.Log($"Receiver activated by pulse with message: {message}");
        // Do your logic here

        this.message = message;
    }

    public void OnPulse()
    {
        // Do nothing
    }
}
