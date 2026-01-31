using UnityEngine;

public class MovementPlayer : MonoBehaviour
{

    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // izquierda/derecha
        movement.y = Input.GetAxisRaw("Vertical");   // arriba/abajo
        movement.Normalize();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }



}
