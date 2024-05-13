using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBall : MonoBehaviour
{
    [SerializeField] private bool RollRight = true;
    [SerializeField] private float forceMagnitude = 2f;
    [SerializeField] private float initialPush = 5f;

    private bool shouldStartRolling = false;
    private bool hasReceivedInitialPush = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (shouldStartRolling)
        {
            if (!hasReceivedInitialPush)
            {
                // Apply an initial push only once
                Vector2 initialForceDirection = RollRight ? Vector2.right : Vector2.left;
                rb.AddForce(initialForceDirection * initialPush, ForceMode2D.Impulse);
                hasReceivedInitialPush = true;
            }

            Vector2 forceDirection = RollRight ? Vector2.right : Vector2.left;
            rb.AddForce(forceDirection * forceMagnitude);
        }
    }

    public void StartRolling()
    {
        shouldStartRolling = true;
    }
}
