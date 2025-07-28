using UnityEngine;

public class ConsumeMachine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("McGibble"))
        {
            int price = col.gameObject.GetComponent<McGibble>().salePrice;
            McGibbleTracker.Remove(col.gameObject);
            GameInfoManager.Instance.AddMoney(price);
            Destroy(col.gameObject);
        }
    }
}
