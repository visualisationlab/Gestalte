using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float rotationSpeed = 50f; // Degrees per second

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
