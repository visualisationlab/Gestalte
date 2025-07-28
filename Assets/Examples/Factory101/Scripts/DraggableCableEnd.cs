using UnityEngine;
using System.Collections;

public class DraggableCableEnd : Draggable
{
    [SerializeField] private LayerMask connectorLayer;
    [SerializeField] private float snapCheckRadius = 1f;
    private CableConnector connectedTo;
    public float maxCableLength = 5f;
    private Coroutine bounceCoroutine;
    private bool bouncingBack = false;

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
        if (nearby.Length != 0)
        {
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

        // // If nothing found return to startposition
        // if (connectedTo == null)
        // {
        //     Debug.Log("No suitable connector found, returning to start position.");
        //     transform.position = startPosition; // Reset to start position
        // }
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

    public void SendPulseFrom(CableConnector source)
    {
        if (connectedTo == null || connectedTo == source)
            return;

        GameObject targetMachine = connectedTo.gameObject;

        if (targetMachine.TryGetComponent<IPulseReceiver>(out var receiver))
        {
            receiver.OnPulse();
            Debug.Log($"Pulse sent from {source.name} to {targetMachine.name}");
        }
        else
        {
            Debug.LogWarning($"{targetMachine.name} does not implement IPulseReceiver");
        }
    }

    public void SendPulse()
    {
        if (connectedTo == null)
        {
            Debug.LogWarning("Cable is not connected to any receiver.");
            return;
        }

        GameObject target = connectedTo.parentMachine;

        if (target.TryGetComponent<IPulseReceiver>(out var receiver))
        {
            receiver.OnPulse();
            Debug.Log($"Pulse sent from {name} to {target.name}");
        }
        else
        {
            Debug.LogWarning($"{target.name} does not implement IPulseReceiver.");
        }
    }

    public override void UpdateDragging()
    {
        base.UpdateDragging(); // Keep base drag behavior

        if (lengthFromTo > maxCableLength && !bouncingBack)
        {
            Debug.LogWarning($"Cable end {name} dragged too far: {lengthFromTo} > {maxCableLength}");
            NoFurtherDragging();
            return;
        }
    }

    private void ResetDragging()
    {
        StopDragging();
        startPosition = transform.position; // Reset start position
        lengthFromTo = 0f;
        transform.rotation = Quaternion.identity; // Reset rotation
    }

    private void NoFurtherDragging()
    {
        base.StopDragging();

        Vector3 origin = connectedTo != null ? connectedTo.transform.position : startPosition;
        Vector3 directionBack = (origin - transform.position).normalized;

        // Stop previous bounce if it's running
        if (bounceCoroutine != null)
            StopCoroutine(bounceCoroutine);

        bouncingBack = true;
        bounceCoroutine = StartCoroutine(BounceBack(startPosition));
    }

    private IEnumerator BounceBack(Vector3 targetPos)
    {
        float duration = 0.2f; // Duration of bounce
        float elapsed = 0f;

        Vector3 start = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // Ensure it ends exactly
        lengthFromTo = Vector3.Distance(startPosition, transform.position);
        bouncingBack = false;
        transform.rotation = Quaternion.identity; // Reset rotation
    }
}
