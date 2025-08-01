using UnityEngine;
using UnityEngine.AI;

public class RotateAround : MonoBehaviour
{
    // degrees per second around Z by default
    public Vector3 angularVelocity = new Vector3(0f, 0f, 90f);


    private Transform targetTransform;
    private RectTransform rectTransform;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Prefer RectTransform if present, else use regular transform
        targetTransform = rectTransform != null ? (Transform)rectTransform : transform;

        if (spriteRenderer == null)
            Debug.LogWarning("RotateAround: no SpriteRenderer found; continuing anyway.");
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        targetTransform.Rotate(angularVelocity * deltaTime, Space.Self);
    }
}