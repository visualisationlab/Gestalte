using UnityEngine;

public class Hover : MonoBehaviour
{
    public float amplitude = 0.3f;     // Height of the hover
    public float frequency = 2f;       // Speed of the hover
    public float bias = 0.2f;          // How much to stick to the bottom (0 = centered, 1 = max stick)

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float rawSin = Mathf.Sin(Time.time * frequency * Mathf.PI * 2); // Sine wave from -1 to 1
        float skewed = (Mathf.Sin(rawSin * Mathf.PI * 0.5f) + 1) / 2;   // Skew it to favor the bottom
        float hoverOffset = skewed * amplitude * (1 - bias);
        transform.position = new Vector3(startPos.x, startPos.y + hoverOffset - amplitude * bias, startPos.z);
    }
}
