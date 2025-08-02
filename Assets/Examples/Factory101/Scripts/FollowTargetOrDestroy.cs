using UnityEngine;
public class FollowTargetOrDestroy : MonoBehaviour
{
    public Transform followTarget;

    void LateUpdate()
    {
        if (followTarget != null)
        {
            transform.position = followTarget.position;
        }
        else
        {
            // If for whatever reason the transform is not set we destroy this object
            Destroy(gameObject);
        }
    }
}
