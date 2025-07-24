using System;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
   public Transform target;
   
   public float smoothSpeed = 0.125f;
   public Vector3 offset;
   private float zPos;

   private void Start()
   {
       zPos = transform.position.z;
   }

   //this script follows the target smoothly for 2D
   void LateUpdate()
   {
       if (target == null)
       {
           Debug.LogError("CameraTracking: Target is not assigned.");
           return;
       }

       Vector3 desiredPosition = target.position + offset;
       Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
       transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, zPos);
   }
}
