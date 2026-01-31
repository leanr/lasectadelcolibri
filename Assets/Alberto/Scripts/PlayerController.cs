using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;

    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentContaminationLevel;
    public float maxHealth = 100f;
    public float maxContaminationLevel = 100f;
    public float contaminationDurationInMinutes = 5f;

    public bool isInContaminationZone = true;

    public float runSpeedMultiplier = 1.5f;
    public float crouchSpeedMultiplier = 0.5f;

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

    public void ApplyEnvironmentalContamination()
    {
        if (currentContaminationLevel > 0 && isInContaminationZone)
        {
            // Calculamos cuánto debe bajar por segundo: (Valor Inicial / (Minutos * 60 segundos))
            float reductionPerSecond = 100f / (contaminationDurationInMinutes * 60f);

            // Restamos el valor proporcional al tiempo que ha pasado desde el último frame
            currentContaminationLevel -= reductionPerSecond * Time.deltaTime;

            // Evitamos que baje de 0
            if (currentContaminationLevel < 0)
            {
                currentContaminationLevel = 0;
            }
            Debug.Log(currentContaminationLevel);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Hardcoded physics settings to ensure it works Top-Down
        rb.gravityScale = 0f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        currentHealth = maxHealth;
        currentContaminationLevel = maxContaminationLevel;
        isInContaminationZone = true;

        torch = GetComponentInChildren<TorchController>();
    }

    private void Update()
    {
        // Torch detection
        if (Input.GetKeyDown(KeyCode.E))
        {
            torch.ToggleTorch();
        }

        ApplyEnvironmentalContamination();

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    // consigo los elementos con los que está colisionando el jugador
        //    // si hay uno del tipo interactuable
        //    // interactable.RunUtility()
        //}
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

        // Lógica de Multiplicadores de Velocidad
        float speedMultiplier = 1.0f; // Velocidad normal por defecto

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedMultiplier = 1.5f; // Sprint
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            speedMultiplier = 0.5f; // Sigilo / Agachado
        }

        // Aplicar movimiento con la velocidad final
        float finalSpeed = speed * speedMultiplier;
        rb.MovePosition(rb.position + direction * finalSpeed * Time.fixedDeltaTime);

        if (torch != null)
        {
            torch.RotateTorch(direction);
        }
    }
}
