using Examples.Factory101.Scripts.Input;
using UnityEngine;
using UnityEngine.Events;

public class ConsumeMachine : MonoBehaviour, IBlockPlacement
{
    public SpriteRenderer arrow;

    public bool nonStartMcGibbleMade;
    public UnityEvent onFirstNotStartMcGibble;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("McGibble"))
        {
            var mcGibble = col.gameObject.GetComponent<McGibble>();
            if (!nonStartMcGibbleMade && mcGibble.description.name != "Yki")
            {
                nonStartMcGibbleMade = true;
                onFirstNotStartMcGibble.Invoke();
            }
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
