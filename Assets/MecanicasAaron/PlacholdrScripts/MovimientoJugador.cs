using UnityEngine;
public class MovimientoJugador : MonoBehaviour
{
[SerializeField] private float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Resetear movimiento cada frame
        movement = Vector2.zero;
        isMoving = false;
        
        // Detección de teclas - literalmente escritas
        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            movement.y = 1;
            isMoving = true;
        }
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            movement.y = -1;
            isMoving = true;
        }
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            movement.x = -1;
            isMoving = true;
        }
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            movement.x = 1;
            isMoving = true;
        }
        
        // También puedes usar KeyCode directamente
        if (Input.GetKey(KeyCode.W)) movement.y = 1;
        if (Input.GetKey(KeyCode.S)) movement.y = -1;
        if (Input.GetKey(KeyCode.A)) movement.x = -1;
        if (Input.GetKey(KeyCode.D)) movement.x = 1;
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        GestorInteractuar.GestorColision(c ,true);
    }

    void OnTriggerExit2D(Collider2D c)
    {
        GestorInteractuar.GestorColision(c ,false);
    }

    
}
