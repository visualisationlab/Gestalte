using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class DraggableBase2D : MonoBehaviour, IDraggable
{
    protected bool isDragging;
    private Vector3 dragOffset;

    public bool IsDragging => isDragging;

    public UnityEvent OnDraggingStarted;
    public UnityEvent OnDraggingStopped;

    public virtual void StartDrag(Vector3 worldPointerPosition)
    {
        isDragging = true;
        dragOffset = transform.position - worldPointerPosition;
        OnDraggingStarted.Invoke();
    }

    public virtual void UpdateDrag(Vector3 worldPointerPosition)
    {
        if (!isDragging) return;

        Vector3 target = worldPointerPosition + dragOffset;
        target.z = transform.position.z; // preserve original z if needed
        ApplyDragMovement(target);
    }

    public virtual void StopDrag()
    {
        if (!isDragging) return;
        isDragging = false;
        OnDraggingStopped.Invoke();
    }

    /// <summary>Override to control how the object moves during drag (snap, constraints, smoothing, etc).</summary>
    private void ApplyDragMovement(Vector3 targetWorldPos)
    {
        transform.position = targetWorldPos;
    }
}