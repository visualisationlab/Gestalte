using UnityEngine;
using System.Collections;
using Examples.Factory101.Scripts;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class DraggableCableEnd : DraggableBase2D
{
    [SerializeField] private LayerMask connectorLayer;
    [SerializeField] private float snapCheckRadius = 1f;
    [SerializeField] private SpriteRenderer plug;
    private CableConnector connectedTo;
    public float maxCableLength = 5f;
    private Coroutine bounceCoroutine;
    private bool bouncingBack = false;
    
    [SerializeField] private float rotationSpeed = 720f; // degrees per second
    [SerializeField] protected float rotationOffset = 0f;  // degrees, applied to rotation
    [SerializeField] protected float lengthFromTo = 0f; // Enable to see debug logs
    protected Vector3 startPosition;
    
    private Vector3 previousWorldPos;

    public int plugDragSortingOrder = 700;
    public int plugStandardSortingOrder = 3;

    private void Start()
    {
        startPosition = transform.position;
    }

    public override void StartDrag(Vector3 worldPointerPosition)
    {
        base.StartDrag(worldPointerPosition);
        isDragging = true;
        plug.sortingOrder = plugDragSortingOrder;
        // Detach if currently snapped
        if (connectedTo != null)
        {
            connectedTo.DetachCable(this);
            connectedTo = null;
        }
    }
    
    public override void StopDrag()
    {
        base.StopDrag();
        isDragging = false;
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
                    plug.sortingOrder = plugStandardSortingOrder;
                    return;
                }
            }
        }
        //nothing found, snap back
        transform.position = startPosition;
        plug.sortingOrder = plugStandardSortingOrder;
    }
    
    public void SnapTo(Transform target, Quaternion snapRotation)
    {
        transform.position = new Vector3(target.position.x, target.position.y, 0f);
    
        // Apply rotation offset around Z axis
        Quaternion offsetRotation = Quaternion.Euler(0, 0, rotationOffset);
        transform.rotation = snapRotation * offsetRotation;
        StartCoroutine(FlashCable());
    }

    // Optional: draw snap radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, snapCheckRadius);
    }

    public void SendPulse(McGibbleDescription message)
    {
        if (connectedTo == null)
        {
            Debug.LogWarning("Cable is not connected to any receiver.");
            return;
        }

        GameObject target = connectedTo.parentMachine;

        if (target.TryGetComponent<IPulseReceiver<McGibbleDescription>>(out var receiver))
        {
            PlayPulseEffect();
            receiver.OnPulse(message);
            Debug.Log($"Pulse sent from {name} to {target.name} with message: {message}");
        }
        else
        {
            Debug.LogWarning($"{target.name} does not implement IPulseReceiver.");
        }
    }
    
    public void UpdateDragging()
    {
        if (!isDragging) return;
        Vector3 currentWorldPos = GetWorldPointer();
        Vector3 delta = currentWorldPos - previousWorldPos;

        // avoid zero-length
        if (delta.sqrMagnitude > 0.0001f)
        {
            Vector3 rotatedDelta = new Vector3(delta.y, -delta.x, 0f); // 90° rotate
            float targetAngle = Mathf.Atan2(rotatedDelta.y, rotatedDelta.x) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(0f, 0f, targetAngle);

            if (float.IsInfinity(rotationSpeed))
            {
                transform.rotation = targetRot; // instant
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 
                    Mathf.Clamp01(Time.deltaTime * rotationSpeed));
            }

            previousWorldPos = currentWorldPos;
        }
    }
    
    private Vector3 GetWorldPointer()
    {
        Vector3 screenPos = Mouse.current.position.ReadValue();
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z); // for typical orthographic or z-offset
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
        world.z = 0f; // keep on 2D plane
        return world;
    }

    void Update()
    {
        UpdateDragging();
    }

    [ContextMenu("Play Pulse Effect")]
    public void PlayPulseEffect()
    {
        SpawnSparks(transform.position);
        SpawnSparks(startPosition);
    }

    private void SpawnSparks(Vector3 position)
    {
        int sparkCount = 6;
        float sparkSpeed = 2f;
        float sparkDuration = 0.5f;

        for (int i = 0; i < sparkCount; i++)
        {
            GameObject spark = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(spark.GetComponent<Collider>());

            spark.name = "Spark";
            spark.transform.position = position;
            spark.transform.localScale = new Vector3(0.05f, 0.2f, 1f);
            spark.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            var renderer = spark.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Sprites/Default"));
            renderer.material.color = Color.yellow;

            Vector3 direction = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0
            ).normalized;

            StartCoroutine(MoveAndFadeSpark(spark, direction * sparkSpeed, sparkDuration));
        }
    }
    private IEnumerator MoveAndFadeSpark(GameObject spark, Vector3 velocity, float duration)
    {
        float elapsed = 0f;
        Vector3 start = spark.transform.position;

        Material mat = spark.GetComponent<Renderer>().material;
        Color initialColor = mat.color;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            spark.transform.position = start + velocity * t;

            // Fade out
            mat.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f - t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(spark);
    }
    private IEnumerator FlashCable()
    {
        LineRenderer line = GetComponent<LineRenderer>();

        Color originalStart = line.startColor;
        Color originalEnd = line.endColor;
        float originalWidth = line.widthMultiplier;

        // Flash to red and increase width
        line.startColor = Color.green;
        line.endColor = Color.green;
        line.widthMultiplier = 1.2f;

        yield return new WaitForSeconds(0.2f);

        // Restore original color and width
        line.startColor = originalStart;
        line.endColor = originalEnd;
        line.widthMultiplier = originalWidth;
    }
}
