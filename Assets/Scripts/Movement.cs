using UnityEngine;

public class Movement : MonoBehaviour
{

    public float speed = 1f;
    public bool canMove = true;
    private Rigidbody2D rb;
    public Transform player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        if (player == null) return;

        // Vector que apunta al jugador
        Vector2 direction = (player.position - transform.position).normalized;

        // Mueve usando MovePosition para respetar colisiones
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pocion"))
        {

           
        }


    }



}
