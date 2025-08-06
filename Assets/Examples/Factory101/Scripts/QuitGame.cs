using Examples.Factory101.Scripts;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
   public void Quit()
   {
      StatApi.Instance.StartLogging(new LogPayload
      {
         playTime = $"{Time.time}",
         message = "Quit Game Called"
      });
      Application.Quit();
   }
}
