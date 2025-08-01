using UnityEngine;

public class BuildToolGhost : MonoBehaviour
{
   [SerializeField] private GameObject currentPlaceablePrefab;

   [SerializeField] private GhostMachineReference referenceList;
   private GameObject placeableGhost;

   private int rotation;
   private int rotateAmount = 90;
   
   public void SetPlaceablePrefab(GameObject prefab)
   {
      rotation = 0;
      currentPlaceablePrefab = prefab;
      var ghostVersion = referenceList.GetTarget(currentPlaceablePrefab);
      placeableGhost = Instantiate(ghostVersion, transform);
      placeableGhost.transform.localPosition = Vector3.zero;
   }

   public void PlaceCurrent()
   {
      var buyable = currentPlaceablePrefab.GetComponent<IBuyable>();

      if (buyable.GetPrice() <= GameInfoManager.Instance.GetMoney())
      {
         var realPlaceable = Instantiate(currentPlaceablePrefab, transform.position, Quaternion.identity);
         realPlaceable.transform.Rotate(Vector3.forward, rotation);
         AudioManager.Instance.Pluck();
         GameInfoManager.Instance.AddMoney(-buyable.GetPrice());
      }
      else
      {
         //TODO Cant place
      }
   }

   public void RemovePlaceablePrefab()
   {
      if(placeableGhost != null){
         Destroy(placeableGhost);
      }
   }

   public void TryRotate()
   {
      if (placeableGhost.GetComponent<GhostAllowRotation>())
      {
         rotation += rotateAmount;
         placeableGhost.transform.Rotate(Vector3.forward, rotateAmount);
      }
   }
   
}
