using UnityEngine;

public class BuildToolGhost : MonoBehaviour
{
   private GameObject currentGhost;
   [SerializeField] private Transform ghostHolder;
   [SerializeField] private GameObject rotationIcon;
   public void SetGhost(GameObject ghost, bool canRotate)
   {
      RemoveGhost();
      currentGhost = Instantiate(ghost, ghostHolder);
      rotationIcon.SetActive(canRotate);
   }

   public void RemoveGhost()
   {
      Destroy(currentGhost);
      currentGhost = null;
   }

   public void RotateGhost(float rotation)
   {
      currentGhost.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
   }
   
}
