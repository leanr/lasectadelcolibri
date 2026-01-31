using UnityEngine;

public class MovementPlayer : MonoBehaviour
{

    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;           // Sin gravedad
        rb.freezeRotation = true;      // No rotar
    }

    void Update()
    {
        // Captura input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
    }

    void FixedUpdate()
    {
        // Movimiento f�sico con velocidad
        rb.linearVelocity = movement * speed;
    }


}
