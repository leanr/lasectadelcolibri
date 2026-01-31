using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    public float currentContaminationDurationInMinutes;
    public float defaultContaminationDurationInMinutes = 5f;
    public float amplifiedContaminationLevelMultiplier = 1.5f;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 66.6f; 
    public float staminaRegenRate = 20f;   // Se rellena en 5 segundos

    public bool isInContaminationZone = true;
    [HideInInspector]
    public bool isNightVisionOn;

    public float runSpeedMultiplier = 1.5f;
    public float crouchSpeedMultiplier = 0.5f;
    private bool isExhausted = false;

    public Light2D globalLight;

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
            float reductionPerSecond = 100f / (currentContaminationDurationInMinutes * 60f);

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

    public float GetFinalSpeed(Vector2 direction)
    {

        float speedMultiplier = 1.0f; // Velocidad normal por defecto
        bool isMoving = direction.sqrMagnitude > 0;
        // --- LÓGICA DE ESTAMINA ---

        // 1. Control de fatiga: Si llega a 0, se agota. Si llega al máximo, se recupera.
        if (currentStamina <= 0)
        {
            isExhausted = true;
        }
        else if (currentStamina >= maxStamina) // this forces the current stamina to be equal to the maxStamina value. Waits until fully recovers the stamina
        {
            isExhausted = false;
        }

        // 2. ¿Puede sprintar? (Pulsa Shift + Se mueve + NO está agotado)
        if (Input.GetKey(KeyCode.LeftShift) && isMoving && !isExhausted)
        {
            speedMultiplier = 1.5f;
            currentStamina -= staminaDrainRate * Time.fixedDeltaTime;
        }
        else
        {
            // Recuperación (ocurre siempre que no estemos sprintando)
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.fixedDeltaTime;
            }

            // Multiplicador de sigilo (solo si no estamos intentando correr)
            if (Input.GetKey(KeyCode.LeftControl))
            {
                speedMultiplier = 0.5f;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // Aplicar movimiento con la velocidad final
        float finalSpeed = speed * speedMultiplier;

        return finalSpeed;
    }

    public void ToggleNightVision()
    {
        // Deactivate night vision
        if (isNightVisionOn)
        {
            globalLight.color = Color.white;
            globalLight.intensity = 0.4f;

            currentContaminationDurationInMinutes = defaultContaminationDurationInMinutes / amplifiedContaminationLevelMultiplier;
        }
        // Activate night vision
        else
        {
            // switch off the torch
            if (torch.isOn)
            {
                torch.ToggleTorch();
            }

            globalLight.color = new Color32(145, 255, 119, 255);
            globalLight.intensity = 1;

            currentContaminationDurationInMinutes = defaultContaminationDurationInMinutes / amplifiedContaminationLevelMultiplier;
        }
        isNightVisionOn = !isNightVisionOn;
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
        currentStamina = maxStamina;
        isInContaminationZone = true;
        currentContaminationDurationInMinutes = defaultContaminationDurationInMinutes;
        isNightVisionOn = false;
        torch = GetComponentInChildren<TorchController>();   
    }

    private void Update()
    {
        // Torch detection
        if (Input.GetKeyDown(KeyCode.E) && !isNightVisionOn)
        {
            torch.ToggleTorch();
        }

        // Torch detection
        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleNightVision();
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

        // Check if we're running or crouching
        float finalSpeed = GetFinalSpeed(direction);
        rb.MovePosition(rb.position + direction * finalSpeed * Time.fixedDeltaTime);

        if (torch != null)
        {
            torch.RotateTorch(direction);
        }
    }
}
