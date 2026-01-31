using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;

    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentContaminationLevel;
    public float maxHealth;
    public float maxContaminationLevel;

    [HideInInspector]
    public TorchController torch;

    [HideInInspector]
    public static PlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Hardcoded physics settings to ensure it works Top-Down
        rb.gravityScale = 0f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //Initialize player attributes
        maxHealth = 100f;
        maxContaminationLevel = 100f;

        torch = GetComponent<TorchController>();

    }

    void FixedUpdate()
    {
        Vector2 direction = Vector2.zero;

        // Hardcoded WASD Detection
        if (Input.GetKey(KeyCode.W)) direction.y = 1;
        if (Input.GetKey(KeyCode.S)) direction.y = -1;
        if (Input.GetKey(KeyCode.A)) direction.x = -1;
        if (Input.GetKey(KeyCode.D)) direction.x = 1;

        // Normalizing manually to prevent fast diagonal movement
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        // Torch detection
        if (Input.GetKey(KeyCode.E))
        {
            torch.ToggleTorch();
        }

        if (Input.GetKey(KeyCode.F))
        {
            // consigo los elementos con los que está colisionando el jugador
            // si hay uno del tipo interactuable
            // interactable.RunUtility()
        }

        // Hardcoded speed value: 5.0f
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
}
