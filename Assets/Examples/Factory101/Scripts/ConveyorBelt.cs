using UnityEngine;

public class ConveyorBelt : MonoBehaviour, IBuyable
{
    public float pushStrength = 5f;
    public int basePrice;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<McGibble>())
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.AddForce(transform.right * pushStrength);  // Or transform.up depending on your belt direction
        }
    }

    public int GetPrice()
    {
        return basePrice;
    }
}