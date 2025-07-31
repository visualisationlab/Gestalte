using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float pushStrength = 5f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<McGibble>())
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.AddForce(transform.right * pushStrength);  // Or transform.up depending on your belt direction
        }
    }
}