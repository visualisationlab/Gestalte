using UnityEngine;

public class NameSensor : MonoBehaviour
{
    public string nameDetected;
    public void OnTriggerStay2D(Collider2D col)
    {
        nameDetected = col.gameObject.name;
    }
    
    public void OnTriggerExit2D(Collider2D col)
    {
        nameDetected = "";
    }
    
}
