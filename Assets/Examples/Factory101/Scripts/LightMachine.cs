using UnityEngine;
using Mediator;

public class LightMachine : MonoBehaviour, IPulseReceiver<bool>
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

    public void OnPulse(bool message)
    {
        Debug.Log("Receiver activated by pulse!");
        // Do your logic here
        SetState(true);
    }
}
