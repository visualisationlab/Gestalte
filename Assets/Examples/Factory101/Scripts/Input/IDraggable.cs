using UnityEngine;

public interface IDraggable
{
    void StartDrag(Vector3 worldPosition);

    void UpdateDrag(Vector3 worldPosition);

    void StopDrag();
}