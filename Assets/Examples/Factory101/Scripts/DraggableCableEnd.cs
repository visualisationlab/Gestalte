using UnityEngine;

public class DraggableCableEnd : Draggable
{
    [SerializeField] private LayerMask connectorLayer;
    [SerializeField] private float snapCheckRadius = 1f;
    private CableConnector connectedTo;

    public override void StartDragging(Vector3 hitPoint)
    {
        base.StartDragging(hitPoint);

        // Detach if currently snapped
        if (connectedTo != null)
        {
            connectedTo.DetachCable(this);
            connectedTo = null;
        }
    }

    public override void StopDragging()
    {
        base.StopDragging();

        // Try to snap to nearby connector
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, snapCheckRadius, connectorLayer);
        if (nearby.Length == 0)
        {
            Debug.Log("No connectors nearby to snap to.");
            return;
        }
        Debug.Log($"Found {nearby.Length} nearby colliders for snapping.");
        foreach (var col in nearby)
        {
            if (col.TryGetComponent<CableConnector>(out var connector))
            {
                Debug.Log($"Attempting to snap to connector: {connector.name}");
                connector.AttachCable(this);
                connectedTo = connector;
                break;
            }
        }
    }

    public void SnapTo(Transform target, Quaternion snapRotation)
    {
        transform.position = target.position;

        // Apply rotation offset around Z axis
        Quaternion offsetRotation = Quaternion.Euler(0, 0, rotationOffset);
        transform.rotation = snapRotation * offsetRotation;
    }

    public void ReleaseFromSnap()
    {
        // Optional visual logic here
    }

    // Optional: draw snap radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, snapCheckRadius);
    }
}
