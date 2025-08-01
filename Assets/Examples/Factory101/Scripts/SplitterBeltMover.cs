using UnityEngine;

public class SplitterBeltMvoer : MonoBehaviour
{
    public float pushStrength = 5f;
    public Vector2 pushDirection = Vector2.right; // Editable in Inspector

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<McGibble>())
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.AddForce(pushDirection.normalized * pushStrength);
        }
    }
}