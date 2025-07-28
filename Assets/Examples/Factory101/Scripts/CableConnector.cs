using UnityEngine;

public class CableConnector : MonoBehaviour
{
    [SerializeField] private float snapRange = 1f;

    private DraggableCableEnd currentCable;

    public bool IsCableNearby(DraggableCableEnd cable)
    {
        return Vector2.Distance(transform.position, cable.transform.position) <= snapRange;
    }

    public void AttachCable(DraggableCableEnd cable)
    {
        if (currentCable == cable) return;

        currentCable = cable;
        cable.SnapTo(transform);
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

    void Awake()
    {
        // debug this layer this script is attached to
        Debug.Log($"CableConnector Awake: {gameObject.name} on layer {gameObject.layer} ({LayerMask.LayerToName(gameObject.layer)})");
    }
    public bool HasCableAttached => currentCable != null;
}
