using UnityEngine;

public class SimpleSensor : MonoBehaviour
{
    public bool onDetect;
    public GameObject detectedGameObject;
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        onDetect = true;
        detectedGameObject = col.gameObject;
    }
    
    public virtual void OnTriggerExit2D(Collider2D col)
    {
        onDetect = false;
        detectedGameObject = null;
    }
}
