using UnityEngine;

public class ActuatorSensor : MonoBehaviour
{
    public bool onDetect;
    public void OnTriggerEnter2D(Collider2D col)
    {
        onDetect = true;
    }
    
    public void OnTriggerExit2D(Collider2D col)
    {
        onDetect = false;
    }
}
