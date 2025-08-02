using Examples.Factory101.Scripts.Input;
using UnityEngine;

public class ConsumeMachine : MonoBehaviour, IBlockPlacement
{
    public SpriteRenderer arrow;
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

    public void ShowArrow()
    {
        arrow.gameObject.SetActive(true);
    }

    public void HideArrow()
    {
        arrow.gameObject.SetActive(false);
    }
}
