using UnityEngine;

public class ConsumeMachine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("McGibble"))
        {
            var mcGibble = col.gameObject.GetComponent<McGibble>();
            
            //Calculate Sale price
            
            int price = mcGibble.GetCurrentSalePrice();
            McGibbleTracker.Instance.Remove(mcGibble);
            GameInfoManager.Instance.AddMoney(price);
        }
    }
}
