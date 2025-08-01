using UnityEngine;

public class SplitterBeltMvoer : MonoBehaviour
{
    public float pushStrength = 5f;
    public bool up = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<McGibble>())
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            Vector2 direction = up ? (Vector2)transform.up : -(Vector2)transform.up;
            rb.AddForce(direction * pushStrength);  // Or transform.up depending on your belt direction
        }
    }
}