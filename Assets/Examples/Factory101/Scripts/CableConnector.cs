using UnityEngine;

public class CableConnector : MonoBehaviour
{
    [SerializeField] private float snapRange = 1f;
    [SerializeField] private Transform centerPoint;  // drag the machine’s center here
    public GameObject parentMachine;
    private DraggableCableEnd currentCable;

    public bool IsCableNearby(DraggableCableEnd cable)
    {
        return Vector2.Distance(transform.position, cable.transform.position) <= snapRange;
    }

    public void AttachCable(DraggableCableEnd cable)
    {
        if (currentCable == cable) return;

        currentCable = cable;
        Quaternion snapRotation = GetSnapRotation();
        cable.SnapTo(transform, snapRotation);
        Debug.Log("Cable attached to " + name);
    }

    public void DetachCable(DraggableCableEnd cable)
    {
        if (currentCable == cable)
        {
            currentCable = null;
            cable.ReleaseFromSnap();
            Debug.Log("Cable detached from " + name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, snapRange);
    }


    public Quaternion GetSnapRotation()
    {
        if (centerPoint == null)
        {
            Debug.LogWarning("CableConnector: Center point not set. Returning default rotation.");
            return Quaternion.identity;
        }

        Vector2 direction = -(transform.position - centerPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0f, 0f, angle);
    }

    public bool HasCableAttached => currentCable != null;
}
