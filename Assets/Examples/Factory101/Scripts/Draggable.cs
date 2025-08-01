using UnityEngine;
using UnityEngine.InputSystem;

public class Draggable : MonoBehaviour
{
    [Header("Placement")] 
    public bool inGrid;
    public bool keepsOffset;
    
    [Header("Rotation")]
    public bool noRotation = false; // Disable rotation if true
    private Vector3 offset;
    private bool isDragging = false;
    private Camera cam;
    private Vector3 lastPosition;
    [SerializeField] private float rotationSpeed = 720f; // degrees per second
    [SerializeField] protected float rotationOffset = 0f;  // degrees, applied to rotation
    [SerializeField] protected float lengthFromTo = 0f; // Enable to see debug logs
    protected Vector3 startPosition;
    void Start()
    {
        cam = Camera.main;
        startPosition = transform.position;
    }

    public virtual void StartDragging(Vector3 hitPoint)
    {
        if(keepsOffset){
            offset = transform.position - hitPoint;
        }
        
        lastPosition = transform.position;
        isDragging = true;
    }

    public virtual void StopDragging()
    {
        // Debug.Log("Stopped dragging: " + gameObject.name);
        isDragging = false;
    }

    private Vector3 StickToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x),
            Mathf.Round(position.y),
            Mathf.Round(position.z)
        );
    }
    
    public virtual void UpdateDragging()
    {
        if (!isDragging) return;

        // convert mouse to world and apply offset
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 world = cam.ScreenToWorldPoint(mouseScreen);
        world.z = 0f;
        Vector3 targetPos = world + offset;
        transform.position = inGrid ? StickToGrid(targetPos) : targetPos;

        // compute how far we've dragged
        lengthFromTo = Vector3.Distance(startPosition, targetPos);

        if (!noRotation)
        {
            // compute direction based on the *previous* lastPosition
            Vector3 dragDirection = targetPos - lastPosition;
            if (dragDirection.sqrMagnitude > 0.001f)
            {
                float targetAngle = Mathf.Atan2(dragDirection.y, dragDirection.x) * Mathf.Rad2Deg + rotationOffset;
                Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        // now update lastPosition for the next frame
        lastPosition = targetPos;
    }

    void Update()
    {
        UpdateDragging();
    }
}
