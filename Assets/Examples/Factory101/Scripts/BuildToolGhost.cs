using System;
using UnityEngine;

public class BuildToolGhost : MonoBehaviour
{
   private GameObject currentPlaceablePrefab;

   public void SetPlaceablePrefab(GameObject prefab)
   {
      currentPlaceablePrefab = prefab;
   }

   public void PlaceCurrent()
   {
      Debug.Log("PLACE CURRENT NOW REMOVE SELF");
      Instantiate(currentPlaceablePrefab, transform.position, Quaternion.identity);
   }
   
}
