using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;

    //0 no mascara, 1 mascara de gas, 2 mascara de vision
    public static int currentMask = 0;
    [HideInInspector]
    public float currentHealth;
    //[HideInInspector]
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

    public List<GameObject> objetosColsiones = new List<GameObject>();
    public List<GameObject> objetosRecogidos = new List<GameObject>();

    public bool isRunning;
    public bool isCrouching;

    [HideInInspector]
    public static PlayerController instance;

    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider contaminationSlider;

    private float previousGlobalLightIntensity;

    // Variables para el texto flotante
    private GameObject floatingTextObject;
    private Coroutine typingCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("HealthSlider: " + healthSlider);
            Debug.Log("StaminaSlider: " + staminaSlider);
            Debug.Log("ContaminationSlider: " + contaminationSlider);
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
            //Debug.Log(currentContaminationLevel);
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

        // check booleans
        if (speedMultiplier == 0.5f)
        {
            isCrouching = true;
        }
        else if (speedMultiplier == 1.5f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
            isCrouching = false;
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
            globalLight.intensity = previousGlobalLightIntensity;

            currentContaminationDurationInMinutes = defaultContaminationDurationInMinutes;
        }
        // Activate night vision
        else
        {
            previousGlobalLightIntensity = globalLight.intensity;
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

    public void Recoger(GameObject go)
    {
        this.objetosRecogidos.Add(go);
    }

    public bool GastarLlave()
    {
        List<GameObject> objetosACopiar = new List<GameObject>(objetosRecogidos);

        foreach (GameObject e in objetosACopiar)
        {
            if (e.GetComponent<Llave>() != null)
            {
                objetosRecogidos.Remove(e);

                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        GestorInteractuar.GestorColision(c, true);
        objetosColsiones.Add(c.gameObject);
    }

    void OnTriggerExit2D(Collider2D c)
    {
        GestorInteractuar.GestorColision(c, false);
        objetosColsiones.Remove(c.gameObject);
    }

    public void InitializeSliders()
    {
        healthSlider.maxValue = maxHealth;
        staminaSlider.maxValue = maxStamina;
        contaminationSlider.maxValue = maxContaminationLevel;
    }

    public void UpdateSliders()
    {
        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;
        contaminationSlider.value = currentContaminationLevel;
    }

    public void CheckGameOver()
    {
        if (currentHealth <= 0)
        {
           // gameObject.SetActive(false);
        }
    }

    public void CheckContaminationExcess()
    {
        // if (currentContaminationLevel < 25f && !Camera.main.GetComponent<CameraController>().isDistorsionOn)
        // {
        //     Camera.main.GetComponent<CameraController>().ApplyDistorsion();
        // }
        // else if (currentContaminationLevel >= 25f && Camera.main.GetComponent<CameraController>().isDistorsionOn)
        // {
        //     Camera.main.GetComponent<CameraController>().StopDistorsion();
        // }
    }

    // Método único que crea y muestra el texto flotante
    public void ShowFloatingText(string message, float typingSpeed = 0.05f, float displayDuration = 2f, float heightOffset = 2f)
    {
        // Si ya hay una coroutine escribiendo, la detenemos
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            if (floatingTextObject != null)
            {
                Destroy(floatingTextObject);
            }
        }
        
        typingCoroutine = StartCoroutine(CreateAndTypeText(message, typingSpeed, displayDuration, heightOffset));
    }

    private IEnumerator CreateAndTypeText(string message, float typingSpeed, float displayDuration, float heightOffset)
    {
        // Crear el GameObject de texto
        floatingTextObject = new GameObject("FloatingText");
        TextMeshPro textMesh = floatingTextObject.AddComponent<TextMeshPro>();
        
        // Configurar el texto
        textMesh.fontSize = 3;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.color = Color.white;
        textMesh.text = "";
        
        // Posicionar encima del jugador
        floatingTextObject.transform.SetParent(transform);
        floatingTextObject.transform.localPosition = new Vector3(0, heightOffset, 0);
        
        // Escribir letra por letra
        foreach (char letter in message)
        {
            textMesh.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        // Esperar y destruir
        yield return new WaitForSeconds(displayDuration);
        Destroy(floatingTextObject);
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

        InitializeSliders();
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            List<GameObject> objetosACopiar = new List<GameObject>(objetosColsiones);

            foreach (GameObject e in objetosACopiar)
            {
                if (e != null && e.layer == 31) // Verificar que el objeto no sea null
                {
                    if (e.GetComponent<PuzzleInz>() != null)
                    {
                        e.GetComponent<PuzzleInz>().Usar(this);
                        //Todo añadir que se enseñe el puzzle inazunma
                    }else if (e.GetComponent<Candado>() != null)
                    {
                        e.GetComponent<Candado>().Usar(this);
                    }else
                    {
                        e.GetComponent<Interactuable>().Usar(this);
                    }
                }
            }
        }

        UpdateSliders();
        CheckGameOver();
        CheckContaminationExcess();
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