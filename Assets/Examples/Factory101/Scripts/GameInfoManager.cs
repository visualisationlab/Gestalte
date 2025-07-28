using UnityEngine;
using UnityEngine.Events;

public class GameInfoManager : MonoBehaviour
{
   private int money;

   public UnityEvent<int> OnMoneyUpdate;
   
   public static GameInfoManager Instance { get; private set; }
   
   void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(this.gameObject); // or Destroy(this);
         return;
      }

      Instance = this;
      DontDestroyOnLoad(this.gameObject); // optional
   }
   
   public void AddMoney(int amount)
   {
      money += amount;
      OnMoneyUpdate.Invoke(money);
   }
}
