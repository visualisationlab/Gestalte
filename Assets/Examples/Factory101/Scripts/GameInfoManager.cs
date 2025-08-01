using UnityEngine;
using UnityEngine.Events;

public class GameInfoManager : MonoBehaviour
{
   [SerializeField] private int money;

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

   private void Start()
   {
      OnMoneyUpdate?.Invoke(money);
   }

   public void AddMoney(int amount)
   {
      money += amount;
      OnMoneyUpdate.Invoke(money);
   }

   public int GetMoney()
   {
      return money;
   }
   
}
