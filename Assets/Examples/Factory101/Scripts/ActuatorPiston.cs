using UnityEngine;

public class ActuatorPiston: MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction = Vector2.right; // Direction to extend
    public float distance = 1f; // How far to move
    public float speed = 1f; // Units per second

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private Vector2 newPos;
    private bool extending = false;

    public void Extend()
    {
        extending = true;
    }
    
    public void Retract()
    {
        extending = false;
    }
    
    void Start()
    {
        startPosition = rb.position;
        targetPosition = startPosition + direction.normalized * distance;
    }

    void FixedUpdate()
    {
        if (extending)
        {
            newPos = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
        }
        else
        {
            newPos = Vector2.MoveTowards(rb.position, startPosition, speed * Time.fixedDeltaTime);
        }
        rb.MovePosition(newPos);

    }
}