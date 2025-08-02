using UnityEngine;

public class BuildToolGhost : MonoBehaviour
{
   public void SetGhost(GameObject ghost)
   {
      //TODO Set Ghost
   }

   public void RemoveGhost()
   {
      //Remove Ghost
   }
   
   public void SetPlaceablePrefab(GameObject prefab)
   {
      // // Avoid double ghost
      // if (currentPlaceablePrefab == prefab)
      //    return;
      //
      // rotation = 0;
      // currentPlaceablePrefab = prefab;
      // RemovePlaceablePrefab();
      //
      // var ghostVersion = referenceList.GetTarget(currentPlaceablePrefab);
      // placeableGhost = Instantiate(ghostVersion, transform);
      // placeableGhost.transform.localPosition = Vector3.zero;
   }

   public void Place(GameObject gameObject)
   {
      // var buyable = currentPlaceablePrefab.GetComponent<IBuyable>();
      //
      // if (buyable.GetPrice() <= GameInfoManager.Instance.GetMoney())
      // {
      //    var realPlaceable = Instantiate(currentPlaceablePrefab, transform.position, Quaternion.identity);
      //    realPlaceable.transform.Rotate(Vector3.forward, rotation);
      //    AudioManager.Instance.Pluck();
      //    GameInfoManager.Instance.AddMoney(-buyable.GetPrice());
      // }
      // else
      // {
      //    //TODO Cant place
      // }
   }

   public void RemovePlaceablePrefab()
   {
      // if(placeableGhost != null){
      //    Destroy(placeableGhost);
      // }
   }

   public void TryRotate()
   {
      // if (placeableGhost.GetComponent<GhostAllowRotation>())
      // {
      //    rotation += rotateAmount;
      //    placeableGhost.transform.Rotate(Vector3.forward, rotateAmount);
      // }
   }
   
}
