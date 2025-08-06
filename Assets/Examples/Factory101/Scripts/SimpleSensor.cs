using UnityEngine;
using UnityEngine.Events;

public class SimpleSensor : MonoBehaviour
{
    public bool onDetect;
    public GameObject detectedGameObject;
    public bool ignoreTriggerStay;

    public UnityEvent<GameObject> OnTriggerEnter;
    public UnityEvent OnTriggerExit;
    
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        OnTriggerEnter.Invoke(col.gameObject);
        onDetect = true;
        detectedGameObject = col.gameObject;
    }
    
    public virtual void OnTriggerExit2D(Collider2D col)
    {
        onDetect = false;
        OnTriggerExit.Invoke();
        detectedGameObject = null;
    }
    
    public virtual void OnTriggerStay2D(Collider2D col)
    {
        if(ignoreTriggerStay) return;
        onDetect = true;
        detectedGameObject = col.gameObject;
    }
}
