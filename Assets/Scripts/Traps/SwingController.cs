using UnityEngine;

public class SwingController : MonoBehaviour
{
    public float forceMagnitude = 2f; // De grootte van de kracht die constant wordt toegepast

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() // Gebruik FixedUpdate voor fysica-updates
    {
        if (rb.velocity != Vector2.zero) // Controleer of de bal beweegt
        {
            Vector2 forceDirection = rb.velocity.normalized; // Bepaal de richting van de beweging
            rb.AddForce(forceDirection * forceMagnitude); // Pas een constante kracht toe in die richting
        }
    }
}
