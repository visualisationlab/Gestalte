using UnityEngine;

public class BuildToolGhost : MonoBehaviour
{
   private GameObject currentGhost;
   [SerializeField] private Transform ghostHolder;
   public void SetGhost(GameObject ghost)
   {
      currentGhost = Instantiate(ghost, ghostHolder);
   }

   public void RemoveGhost()
   {
      Destroy(currentGhost);
      currentGhost = null;
   }
   
}
