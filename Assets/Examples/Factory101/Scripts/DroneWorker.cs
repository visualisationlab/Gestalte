using UnityEngine;
using System.Collections;
public class DroneWorker : MonoBehaviour
{
    public float speed = 5f;
    private GameObject targetObject;
    private Transform dropPoint;
    private System.Action<DroneWorker> onComplete;
    public SpriteRenderer spriteRenderer;
    public Transform attachPoint;

    // Threshold to avoid jitter when almost vertically aligned
    private const float flipEpsilon = 0.01f;

    public void StartJob(GameObject itemToMove, Transform dropTarget, System.Action<DroneWorker> onCompleteCallback)
    {
        targetObject = itemToMove;
        dropPoint = dropTarget;
        onComplete = onCompleteCallback;
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        // Move to item
        while (targetObject != null && Vector3.Distance(transform.position, targetObject.transform.position) > 0.1f)
        {
            FlipTowards(targetObject.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, speed * Time.deltaTime);
            yield return null;
        }

        if (targetObject == null)
        {
            Debug.LogWarning("DroneWorker: target was destroyed before pickup. Returning to pool.");
            onComplete?.Invoke(this);
            yield break;
        }

        // Attach item
        targetObject.transform.SetParent(attachPoint);
        var rb = targetObject.GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

        // Move to drop point
        Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
        Vector3 finalDrop = dropPoint.position + randomOffset;

        while (targetObject != null && Vector3.Distance(transform.position, finalDrop) > 0.1f)
        {
            FlipTowards(finalDrop);
            transform.position = Vector3.MoveTowards(transform.position, finalDrop, speed * Time.deltaTime);
            yield return null;
        }

        if (targetObject == null)
        {
            Debug.LogWarning("DroneWorker: target was destroyed mid-flight. Returning to pool.");
            onComplete?.Invoke(this);
            yield break;
        }

        // Drop
        targetObject.transform.SetParent(null);
        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;

        targetObject = null;
        onComplete?.Invoke(this);
    }

    private void FlipTowards(Vector3 destination)
    {
        if (spriteRenderer == null) return;
        float dx = destination.x - transform.position.x;
        if (Mathf.Abs(dx) < flipEpsilon) return; // no meaningful horizontal direction
        // Assumes the sprite faces right by default; flipX = true to face left.
        spriteRenderer.flipX = dx < 0f;
    }

}
