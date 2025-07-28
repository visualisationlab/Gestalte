using UnityEngine;

public class ConsumeMachine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("McGibble"))
        {
            McGibbleTracker.Remove(col.gameObject);
            Destroy(col.gameObject);
        }
    }
}
