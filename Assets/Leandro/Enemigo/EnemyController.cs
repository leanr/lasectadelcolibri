using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType
    {
        Normal,
        SensibleALuz,
        SensibleARuido,
        Veloz,
        Inaturdible
    }

    [Header("Tipo de Enemigo")]
    public EnemyType enemyType = EnemyType.Normal;

    [Header("Velocidad por tipo")]
    public float speedNormalMin = 1.5f;
    public float speedNormalMax = 3f;
    public float speedVeloz = 3.5f;

    [Header("Patrulla")]
    public Vector2 patrolCenter;
    public float patrolRadius = 5f;
    public float patrolPointReachDistance = 0.2f;
    public float patrolSpeed = 1.5f;

    [Header("Aturdimiento")]
    public bool aturdido = false;
    public float tiempoAturdido = 2f;

    [Header("Atención")]
    [Range(0f, 100f)]
    public float attention = 0f;          // Barra de atención
    public float attentionMax = 100f;
    public float incrementPerSecond = 3f; // aumento por segundo
    public float decrementPerSecond = 5f;  // disminución por segundo
    public float maxSpeedBonus = 2f;      // cuánto aumenta la velocidad (por la atencion)

    [Header("Evasión (Tags)")]
    public float avoidRadius = 4.5f;
    public float avoidSpeedBonus = 5f;

    [Header("Evasión avanzada")]
    public float brakeDistance = 0.8f;
    public float brakeStrength = 0.5f;

    [Header("Detección Player")]
    public bool playerDetected = false;
    public float instantDetectionRange = 2f;
    public float baseVisionRange = 5f;
    public float chaseRange = 5f;   // seguir

    [Header("Alertas Visuales")]
    public GameObject alertAttentionRef; // ! -> ahora es un hijo inactivo inicialmente
    public GameObject alertQuestionRef;   // ? -> ahora es un hijo inactivo inicialmente
    public GameObject alertStunnedRef; // Stars

    [Header("Luz")]
    public bool estaIluminado = false;
    public float lightAttentionBonus = 20f;
    public float soundAttentionBonus = 20f;

    [Header("Ruido")]
    public float noiseDetectionRange = 8f;

    [Header("Player Damage")]
    public float damageOnHit = 10f;
    public float damageCooldown = 1f; // segundos entre daños

    [Header("Debug")]
    public bool drawDebugCircles = true;

    private PlayerController player;
    private Rigidbody2D rb;
    private Transform target;

    private float baseSpeed;

    private Vector2 patrolTarget;
    private bool hasPatrolTarget = false;
    
    private string[] avoidTags;

    private float nextDamageTime = 0f;
    private float finAturdimiento = 0f;

    [Header("Pathfinding")]
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private float pathCellSize = 0.5f;      // Tamaño de celda del grid
    [SerializeField] private float pathRefreshRate = 0.3f;   // Cada cuántos segundos recalcula
    [SerializeField] private float waypointReachDistance = 0.3f; // Cuándo pasa al siguiente punto

    private SimplePathfinder pathfinder;
    private List<Vector2> currentPath;
    private int pathIndex = 0;
    private float pathRefreshTimer = 0f;


    private void Start()
    {
        pathfinder = new SimplePathfinder(pathCellSize, obstacleLayerMask, 0.25f);
        SetBaseSpeedByType();
    }

    void Awake()
    {
        // Initialize rb
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        avoidTags = new string[] { "Obstacle", "FactoryDoor" };
        
        patrolCenter = transform.position;

        // Initialize reference to the player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
            player = playerObj.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (target == null) return;

        // 1. GESTIÓN DE ESTADOS Y TIMERS
        // Comprobamos el tiempo de aturdimiento aquí
        if (aturdido && Time.time >= finAturdimiento)
        {
            ExitStunned();
        }

        // 2. ALERTAS VISUALES (UI/Objetos siempre en Update)
        UpdateStunnedAlert();

        if (aturdido) return;

        // 3. CÁLCULOS DE DISTANCIA Y PERCEPCIÓN
        Vector2 directionToPlayer = target.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        CalculateAttention(distanceToPlayer);

        // 4. LÓGICA DE DETECCIÓN (Cambio de estados)
        if (attention >= attentionMax)
        {
            playerDetected = true;
        }

        if (playerDetected && (attention < attentionMax / 2f || distanceToPlayer >= chaseRange))
        {
            playerDetected = false;
        }

        UpdateAttentionAlert(playerDetected);

        // 5. LÓGICA DE PATRULLA (Solo decidir el punto, no mover)
        if (!playerDetected && !hasPatrolTarget)
        {
            patrolTarget = GetRandomPatrolPoint();
            hasPatrolTarget = true;
        }

        if (drawDebugCircles)
        {
            DrawDebugCircles();
        }
    }

    void FixedUpdate()
    {
        if (aturdido)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (target == null) return;

        Vector2 currentPos = transform.position;
        Vector2 directionToPlayer = (Vector2)target.position - currentPos;

        if (playerDetected)
        {
            Vector2 finalMoveDir = directionToPlayer.normalized;

            if (enemyType == EnemyType.Inaturdible)
            {
                finalMoveDir = GetPathfindingDirection(currentPos, directionToPlayer);
            }

            if (directionToPlayer.sqrMagnitude < 0.01f)
                rb.linearVelocity = Vector2.zero;
            else
            {
                rb.linearVelocity = finalMoveDir * baseSpeed;
                hasPatrolTarget = false;
            }
        }
        else
        {
            Vector2 dirPatrol = patrolTarget - currentPos;
            if (dirPatrol.magnitude <= patrolPointReachDistance)
            {
                hasPatrolTarget = false;
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                rb.linearVelocity = dirPatrol.normalized * patrolSpeed;
            }
        }
    }

    // --- NUEVO MÉTODO ---
    private Vector2 GetPathfindingDirection(Vector2 currentPos, Vector2 directionToPlayer)
    {
        pathRefreshTimer -= Time.fixedDeltaTime;

        if (pathRefreshTimer <= 0f || currentPath == null || currentPath.Count == 0)
        {
            pathRefreshTimer = pathRefreshRate;
            currentPath = pathfinder.FindPath(currentPos, target.position);
            pathIndex = 0;

            // DEBUG
            if (currentPath == null)
                Debug.LogWarning("Pathfinder: Path es NULL - no se encontró camino");
            else
                Debug.Log($"Pathfinder: Path encontrado con {currentPath.Count} waypoints");
        }

        if (currentPath != null && currentPath.Count > 0)
        {
            Vector2 waypoint = currentPath[pathIndex];
            float distToWaypoint = Vector2.Distance(currentPos, waypoint);

            // DEBUG
            Debug.DrawLine(currentPos, waypoint, Color.cyan);
            Debug.Log($"Pathfinder: Yendo al waypoint {pathIndex}: {waypoint} | Distancia: {distToWaypoint}");

            if (distToWaypoint <= waypointReachDistance)
            {
                pathIndex++;
                if (pathIndex >= currentPath.Count)
                {
                    currentPath = null;
                    return directionToPlayer.normalized;
                }
                waypoint = currentPath[pathIndex];
            }

            DebugDrawPath(currentPath);
            return (waypoint - currentPos).normalized;
        }

        Debug.LogWarning("Pathfinder: Usando dirección directa al jugador (sin path)");
        return directionToPlayer.normalized;
    }

    private void DebugDrawPath(List<Vector2> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
            Debug.DrawLine(path[i], path[i + 1], Color.yellow, 0.1f);
    }

    // Modificación necesaria en CalculateAttention para usar DeltaTime correcto
    private void CalculateAttention(float distanceToPlayer)
    {
        float dt = Time.deltaTime; // Usamos deltaTime de Update

        if (distanceToPlayer <= instantDetectionRange)
        {
            attention = attentionMax;
        }
        else if (distanceToPlayer <= baseVisionRange)
        {
            attention += incrementPerSecond * dt;

            // sensible a luz
            if (enemyType == EnemyType.SensibleALuz && player.torch.isOn)
            {
                attention += lightAttentionBonus * dt;
                Debug.Log("Se está aplicando el bonus de detección de luminosidad");
            }

            // Sensible a ruido
            if (enemyType == EnemyType.SensibleARuido && !player.isCrouching)
            {
                float runningMultiplier = player.isRunning ? 2f : 1f;
                attention += soundAttentionBonus * runningMultiplier * dt;
                Debug.Log("Se está aplicando el bonus de sensible a ruido con runningMultiplier " + runningMultiplier);
            }
            // block attention if player is crouching
            else if (enemyType == EnemyType.SensibleARuido && player.isCrouching)
            {
                attention -= incrementPerSecond * dt;
            }
        }
        else
        {
            attention -= decrementPerSecond * dt;
        } 

        attention = Mathf.Clamp(attention, 0f, attentionMax);
    }

    //void FixedUpdate()
    //{
    //    if (CheckStunned()) return;
    //    if (target == null) return;

    //    UpdateAttentionAlert();
    //    UpdateStunnedAlert();

    //    Vector2 directionToPlayer = target.position - transform.position;
    //    float distanceToPlayer = directionToPlayer.magnitude;

    //    CalculateAttention(distanceToPlayer);

    //    // ======================
    //    // SENSIBLE A RUIDO
    //    // ======================
    //    //if (enemyType == EnemyType.SensibleARuido && player.isRunning && heardNoise)
    //    //{
    //    //    MoveTowards(noisePosition, currentVisionRange);

    //    //    print("el enemigo aumenta su velocidad");
    //    //    print("el enemigo se acerca al enemigo");
    //    //    print("pending: accion adicional del enemigo");
    //    //    if (Vector2.Distance(transform.position, noisePosition) < 0.3f) // -> se acerca hasta 0.3 de distancia
    //    //        heardNoise = false;

    //    //    return;
    //    //}

    //    // ======================
    //    // VELOZ
    //    // ======================
    //    //if (enemyType == EnemyType.Veloz)
    //    //{



    //    //}

    //    //----------
    //    // INATURDIBLE/INTELIGENTE (solo modifica la dirección) -> Es inteligente y esquiva 
    //    //----------
    //    //if (enemyType == EnemyType.Inaturdible && directionToPlayer.sqrMagnitude > 0.001f)
    //    //{
    //    //    Vector2 pursueDir = directionToPlayer.normalized;
    //    //    Vector2 evadeDir = Vector2.zero;

    //    //    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, avoidRadius);

    //    //    foreach (Collider2D hit in hits)
    //    //    {
    //    //        foreach (string tag in avoidTags)
    //    //        {
    //    //            if (hit.CompareTag(tag))
    //    //            {
    //    //                Vector2 toObstacle = hit.transform.position - transform.position;

    //    //                // solo esquiva lo que está adelante
    //    //                if (Vector2.Dot(pursueDir, toObstacle.normalized) > 0.3f)
    //    //                {
    //    //                    float strength =
    //    //                        1f - Mathf.Clamp01(toObstacle.magnitude / avoidRadius);

    //    //                    evadeDir -= toObstacle.normalized * strength;
    //    //                }
    //    //            }
    //    //        }
    //    //    }

    //    //    // combinar persecución + evasión
    //    //    directionToPlayer = pursueDir + evadeDir;
    //    //}

    //    // Valores dinámicos -> se usan una vez calculada la atención
    //    currentLightSoundPerceptionRange = baseVisionRange + (attention / attentionMax) * maxVisionBonus;
    //    currentSpeed = baseSpeed + (attention / attentionMax) * maxSpeedBonus;

    //    // --- PERSECUCIÓN ---
    //    // if (distanceToPlayer <= visionRangeActual)
    //    // {

    //    //AGREGO LA ANIMACION de alerta o sonido de deteccion 
    //    bool canSeePlayer = distanceToPlayer <= detectRange;
    //    bool shouldChase = distanceToPlayer <= chaseRange;

    //    // ENTRADA AL RANGO DE VISIÓN
    //    if (canSeePlayer && !playerDetected)
    //    {
    //        playerDetected = true;
    //        attentionMax = 100;

    //        Debug.Log($"{gameObject.name} detectó al player");
    //    }

    //    // SALIDA DEL RANGO DE VISIÓN (opcional)
    //    if (!shouldChase && playerDetected)
    //    {
    //        playerDetected = false;
    //        print("salgo del area de deteccion del enemigo");
    //    }

    //    // movimiento hacia el jugador
    //    if (playerDetected && shouldChase)
    //    {
    //        // si está encima del jugador, se queda quieto
    //        if (directionToPlayer.sqrMagnitude < 0.0001f)
    //        {
    //            rb.linearVelocity = Vector2.zero;
    //            return;
    //        }

    //        // si puede verlo, va hacia él
    //        if (canSeePlayer)
    //        {
    //            Vector2 dir = directionToPlayer.normalized;
    //            rb.linearVelocity = dir * currentSpeed;

    //            hasPatrolTarget = false;
    //        }
    //    }

    //    else
    //    {
    //        // --- PATRULLA --- LOGICA para que los enemigos patrullen por el area
    //        if (!hasPatrolTarget)
    //        {
    //            patrolTarget = GetRandomPatrolPoint();
    //            hasPatrolTarget = true;
    //        }

    //        Vector2 dir = patrolTarget - (Vector2)transform.position;

    //        if (dir.magnitude <= patrolPointReachDistance)
    //        {
    //            hasPatrolTarget = false;
    //            rb.linearVelocity = Vector2.zero;
    //        }
    //        else
    //        {
    //            rb.linearVelocity = dir.normalized * patrolSpeed;
    //        }
    //    }
    //}

    //private void CalculateAttention(float distanceToPlayer)
    //{
    //    // Base - Este es el movimiento cuando apareces en el rango de vision del enemigo 
    //    if (distanceToPlayer <= baseVisionRange)
    //    {
    //        attention += incrementPerSecond * Time.fixedDeltaTime; //aumento la atencion
    //        //print("aumente la atencion del enemigo");
    //    }
    //    else
    //    {
    //        attention -= decrementPerSecond * Time.fixedDeltaTime;
    //        //print("bajo la atencion del enemigo, ");
    //    }

    //    // Enemigo sensible a la luz
    //    if (enemyType == EnemyType.SensibleALuz && IsPlayerInPerceptionRange() && player.torch.isOn)
    //    {
    //        attention += lightAttentionBonus * Time.fixedDeltaTime;
    //        Debug.Log("Atención extra");
    //    }

    //    attention = Mathf.Clamp(attention, 0f, attentionMax);
    //}

    Vector2 GetRandomPatrolPoint()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        return patrolCenter + randomOffset;
    }

    void SetBaseSpeedByType()
    {
        if (enemyType == EnemyType.Veloz)
        {
            baseSpeed = speedVeloz;
        }
        else
        {
            baseSpeed = Random.Range(speedNormalMin, speedNormalMax);
        }

        Debug.Log($"{gameObject.name} -> {enemyType} | baseSpeed: {baseSpeed}");
    }

    void UpdateAttentionAlert(bool playerDetected)
    {
        if (attention >= attentionMax || playerDetected)
        {
            alertQuestionRef.SetActive(false);
            alertStunnedRef.SetActive(false);
            alertAttentionRef.SetActive(true);
        }
        else if (attention > 0f)
        {
            alertQuestionRef.SetActive(true);
            alertStunnedRef.SetActive(false);
            alertAttentionRef.SetActive(false);
        }
        else
        {
            alertQuestionRef.SetActive(false);
            alertAttentionRef.SetActive(false);
        }
    }

    void UpdateStunnedAlert()
    {
        print("entro a tirar alerta de aturdido");
        if (aturdido)
        {
            alertStunnedRef.SetActive(true);
            alertAttentionRef.SetActive(false);
            alertQuestionRef.SetActive(false);
        }
        else
        {
            alertStunnedRef.SetActive(false);
        }
    }

    void EnterStunned()
    {
        aturdido = true;
        finAturdimiento = Time.time + tiempoAturdido;

        ShowStunnedAlert();
    }

    bool CheckStunned()
    {
        if (aturdido)
        {
            if (Time.time >= finAturdimiento)
            {
                ExitStunned();
                Debug.Log("Enemigo recuperado");
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                return true;
            }
        }

        return false;
    }

    void ExitStunned()
    {
        aturdido = false;
        HideStunnedAlert();
    }

    void ShowStunnedAlert()
    {
        alertStunnedRef.SetActive(true);
    }

    void HideStunnedAlert()
    {
        alertStunnedRef.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time < nextDamageTime)
                return;

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                if (player.isNightVisionOn)
                {
                    player.playerAnimator.SetTrigger("damageNightVision");
                }
                else if (player.isMaskOn)
                {
                    player.playerAnimator.SetTrigger("damageMask");
                }
                else
                {
                    player.playerAnimator.SetTrigger("damage");
                }

                print("el player recibe daño");
                if (enemyType == EnemyType.SensibleALuz && player.torch.isOn)
                {
                    player.currentHealth = player.currentHealth - 20;
                    print("hice 20 de daño");
                }
                else
                {
                    player.currentHealth = player.currentHealth - 10;
                    print("hice 10 de daño");
                }

                nextDamageTime = Time.time + damageCooldown;
            }
        }

        if (collision.transform.root.CompareTag("Stunner"))
        {
            //el enemigo se aturde
            print("el enemigo esta aturdido y no puede moverse");
            EnterStunned();
        }

    }

    void DrawDebugCircles()
    {
        DrawCircleDebug(transform.position, baseVisionRange, 30, Color.red);
        DrawCircleDebug(transform.position, chaseRange, 30, Color.green);
        DrawCircleDebug(transform.position, instantDetectionRange, 30, Color.blue);
    }

    void DrawCircleDebug(Vector3 center, float r, int segments, Color color)
    {
        float angle = 0f;
        Vector3 lastPoint = center + new Vector3(Mathf.Cos(0) * r, Mathf.Sin(0) * r, 0);

        for (int i = 1; i <= segments; i++)
        {
            angle += (2f * Mathf.PI) / segments;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0);
            Debug.DrawLine(lastPoint, nextPoint, color);
            lastPoint = nextPoint;
        }
    }

}