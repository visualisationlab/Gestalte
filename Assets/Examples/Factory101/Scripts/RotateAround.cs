using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Vector3 angularVelocity = new Vector3(0f, 0f, 90f);
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
            Debug.LogError("RotateRect requires a RectTransform on the same GameObject.");
    }

    void Update()
    {
        // Apply rotation incrementally (local space)
        rectTransform.localRotation *= Quaternion.Euler(angularVelocity * Time.deltaTime);
    }
}