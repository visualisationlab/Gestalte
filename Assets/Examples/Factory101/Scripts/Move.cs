using System;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector2 direction;
    public Rigidbody2D rb;
    private void FixedUpdate()
    {
        rb.AddForce(direction);
    }
}
