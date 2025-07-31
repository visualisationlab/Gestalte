using UnityEngine;

public class BuildToolGhost : MonoBehaviour
{
   [SerializeField] private GameObject currentPlaceablePrefab;

   public void SetPlaceablePrefab(GameObject prefab)
   {
      currentPlaceablePrefab = prefab;
   }

   public void PlaceCurrent()
   {
      Instantiate(currentPlaceablePrefab, transform.position, Quaternion.identity);
      AudioManager.Instance.Pluck();
   }
   
}
