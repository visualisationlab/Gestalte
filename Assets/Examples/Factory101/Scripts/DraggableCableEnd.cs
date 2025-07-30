using UnityEngine;
using System.Collections;
using Examples.Factory101.Scripts;

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
        StartCoroutine(FlashCable());
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

        if (targetMachine.TryGetComponent<IPulseReceiver<bool>>(out var receiver))
        {
            receiver.OnPulse(true);
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

        if (target.TryGetComponent<IPulseReceiver<bool>>(out var receiver))
        {
            PlayPulseEffect();
            receiver.OnPulse(true);
            Debug.Log($"Pulse sent from {name} to {target.name}");
        }
        else
        {
            Debug.LogWarning($"{target.name} does not implement IPulseReceiver.");
        }
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
