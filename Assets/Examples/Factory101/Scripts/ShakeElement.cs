using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ShakeElement : MonoBehaviour
{
    [Header("Shake parameters")]
    public float baseAmplitude = 10f;
    public float frequency = 25f;
    public float baseDuration = 0.3f;
    public float maxDuration = 1f;
    public bool shakeRotation = true;
    public float maxRotationAngle = 5f;

    private RectTransform rect;
    private Coroutine currentShake;

    // Original baseline captured when the first shake in a burst starts
    private Vector2 originalAnchoredPos;
    private Quaternion originalRot;
    private bool hasBaseline = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Shake(int delta)
    {
        if (delta <= 0) return;

        float intensity = Mathf.Sqrt(delta);
        float amplitude = baseAmplitude * intensity;
        float duration = Mathf.Clamp(baseDuration * intensity, 0f, maxDuration);

        // If no active shake, capture the true original baseline
        if (currentShake == null)
        {
            originalAnchoredPos = rect.anchoredPosition;
            originalRot = rect.localRotation;
            hasBaseline = true;
        }

        // Restart shake (but keep original baseline if already shaking)
        if (currentShake != null)
            StopCoroutine(currentShake);

        currentShake = StartCoroutine(ShakeRoutine(amplitude, duration));
    }

    private IEnumerator ShakeRoutine(float amplitude, float duration)
    {
        if (!hasBaseline)
        {
            originalAnchoredPos = rect.anchoredPosition;
            originalRot = rect.localRotation;
            hasBaseline = true;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            float envelope = 1f - Mathf.Pow(progress, 2f); // ease out

            // Position jitter (organic)
            float x = Mathf.Sin(Time.unscaledTime * frequency * 1.1f) * amplitude * 0.5f;
            float y = Mathf.Cos(Time.unscaledTime * frequency * 1.3f) * amplitude * 0.5f;
            Vector2 offset = new Vector2(x, y) * envelope * 0.1f; // scale to keep it subtle

            rect.anchoredPosition = originalAnchoredPos + offset;

            if (shakeRotation)
            {
                float angle = (Mathf.PerlinNoise(Time.unscaledTime * 1.7f, 0f) * 2f - 1f) * maxRotationAngle * envelope;
                rect.localRotation = originalRot * Quaternion.Euler(0f, 0f, angle);
            }

            yield return null;
        }

        // Fully restore to the original baseline
        rect.anchoredPosition = originalAnchoredPos;
        rect.localRotation = originalRot;
        currentShake = null;
    }
}
