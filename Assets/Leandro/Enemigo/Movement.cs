using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{

    // ======================
    // TIPOS DE ENEMIGO
    // ======================
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
    public float speedNormal = 2.5f;
    public float speedSensibleLuz = 1f;
    public float speedSensibleRuido = 4f;
    public float speedVeloz = 8f;
    public float speedInaturdible = 5f;
    float currentBaseSpeed;



    [Header("Movimiento")]
    //public float speed = 3f;
    private Rigidbody2D rb;
    private Transform target;
    public float baseVisionRange = 5f;


    [Header("Patrulla")]
    public Vector2 patrolCenter;
    public float patrolRadius = 5f;
    public float patrolPointReachDistance = 0.2f;
    private Vector2 patrolTarget;
    private bool hasPatrolTarget = false;
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
    public float maxVisionBonus = 5f;     // cuánto aumenta la visión


    [Header("Evasión (Tags)")]
    public string[] avoidTags;
    public float avoidRadius = 4.5f;
    public float avoidSpeedBonus = 5f;

    [Header("Evasión avanzada")]
    public float brakeDistance = 0.8f;
    public float brakeStrength = 0.5f;



    // ======================
    // LUZ
    // ======================
    [Header("Luz")]
    public bool estaIluminado = false;
    public float lightAttentionBonus = 20f;

    // ======================
    // RUIDO
    // ======================
    [Header("Ruido")]
    public float noiseDetectionRange = 8f;
    Vector2 noisePosition;
    bool heardNoise = false;



    PlayerController playerVision;


    private void Start()
    {
        SetBaseSpeedByType();
    }
    void Awake()
    {

        avoidTags = new string[] { "Obstacle", "FactoryDoor" };

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        patrolCenter = transform.position;


        // Buscar player por tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
           playerVision = playerObj.GetComponent<PlayerController>();

        
       



    }

   

void FixedUpdate()
    {

        if (aturdido)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        


        if (target == null) return;

        Vector2 directionToPlayer = target.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Vector hacia el player
        Vector3 direction = (target.position - transform.position);
        float distance = direction.magnitude;




        // ======================
        // LÓGICA DE LUZ
        // ======================
        if (enemyType == EnemyType.SensibleALuz && playerVision.torch.isOn)
        {
            
            //Agregar codigo de comportamiento de enemigo cuando lo iluminen
            //subo la atencion extra
            attention += lightAttentionBonus * Time.fixedDeltaTime;

        }

        attention = Mathf.Clamp(attention, 0, attentionMax);



        // ======================
        // RUIDO TIENE PRIORIDAD
        // ======================
        if (enemyType == EnemyType.SensibleARuido && playerVision.isRunning && heardNoise)
        {

            //ACA VA LOGICA DE QUE HACE EL ENEMIGO CUANDO ESCUCHA RUIDO

            MoveTowards(noisePosition, currentBaseSpeed);

            print("el enemigo aumenta su velocidad");
            print("el enemigo se acerca al enemigo");
            print("pending: accion adicional del enemigo");
            if (Vector2.Distance(transform.position, noisePosition) < 0.3f)
                heardNoise = false;

            return;
        }


       


        // ======================
        // Veloz
        // ======================
        if (enemyType == EnemyType.Veloz)
        {

           

        }


        ///----
        ///INATURDIBLE
        ///

        if (enemyType == EnemyType.Inaturdible)
        {
            Vector2 pursueDir = directionToPlayer.normalized;
            Vector2 evadeDir = Vector2.zero;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, avoidRadius);

            foreach (Collider2D hit in hits)
            {
                foreach (string tag in avoidTags)
                {
                    if (hit.CompareTag(tag))
                    {
                        Vector2 toObstacle = hit.transform.position - transform.position;
                        float dist = toObstacle.magnitude;

                        // 🔑 solo esquiva lo que está adelante
                        if (Vector2.Dot(pursueDir, toObstacle.normalized) > 0.3f)
                        {
                            float strength = 1f - Mathf.Clamp01(dist / avoidRadius);
                            evadeDir -= toObstacle.normalized * strength * 2f;
                        }
                    }
                }
            }

            Vector2 finalDir = pursueDir + evadeDir;

            // 🔑 frenar si hay riesgo de choque
            float speedMultiplier = evadeDir.sqrMagnitude > 0.1f ? 0.7f : 1f;

            if (finalDir.sqrMagnitude > 0.001f)
            {
                rb.linearVelocity = finalDir.normalized * currentBaseSpeed * speedMultiplier;
                return;
            }
        }






        // Este es el movimiento cuando apareces en el rango de vision del enemigo



        if (distanceToPlayer <= baseVisionRange)
        {
            attention += incrementPerSecond * Time.fixedDeltaTime; //aumento la atencion
            //print("aumente la atencion del enemigo");
        }
        else
        {
            attention -= decrementPerSecond * Time.fixedDeltaTime;
            //print("bajo la atencion del enemigo, ");
        }

        attention = Mathf.Clamp(attention, 0f, attentionMax);

        // Valores dinámicos
        float visionRangeActual =
            baseVisionRange + (attention / attentionMax) * maxVisionBonus;

        float speedActual =
               currentBaseSpeed + (attention / attentionMax) * maxSpeedBonus;

        // --- PERSECUCIÓN ---
        if (distanceToPlayer <= visionRangeActual)
        {
            if (directionToPlayer.sqrMagnitude < 0.0001f)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 dir = directionToPlayer.normalized;
            rb.linearVelocity = dir * speedActual;

            hasPatrolTarget = false;
        }

        else
        {
            // --- PATRULLA --- LOGICA para que los enemigos patrullen por el area
            if (!hasPatrolTarget)
            {
                patrolTarget = GetRandomPatrolPoint();
                hasPatrolTarget = true;
            }

            Vector2 dir = patrolTarget - (Vector2)transform.position;

            if (dir.magnitude <= patrolPointReachDistance)
            {
                hasPatrolTarget = false;
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                rb.linearVelocity = dir.normalized * patrolSpeed;
            }
        }


   



}

    //METODO DE LA LUZ
    void MoveTowards(Vector2 target, float speed)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * speed;
        //agrego el *5 para probar si es muy rapido
    }


    //METODO DEL RUIDO
    public void HearNoise(Vector2 pos, float strength)
    {
        if (enemyType != EnemyType.SensibleARuido) return;

        if (Vector2.Distance(transform.position, pos) > noiseDetectionRange)
            return;

        noisePosition = pos;
        heardNoise = true;
        attention += strength;
    }


    Vector2 GetRandomPatrolPoint()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        return patrolCenter + randomOffset;
    }


    void SetBaseSpeedByType()
    {
        switch (enemyType)
        {
            case EnemyType.SensibleALuz:
                currentBaseSpeed = speedSensibleLuz;
                break;

            case EnemyType.SensibleARuido:
                currentBaseSpeed = speedSensibleRuido;
                break;

            case EnemyType.Veloz:
                currentBaseSpeed = speedVeloz;
                break;

            case EnemyType.Inaturdible:
                currentBaseSpeed = speedInaturdible;
                break;

            default:
                currentBaseSpeed = speedNormal;
                break;
        }

        Debug.Log($"{gameObject.name} -> {enemyType} | baseSpeed: {currentBaseSpeed}");
    }

    /*
     * METODO PARA ATURDIR
    public void Aturdir(float duracion)
    {
        if (!aturdido)
            StartCoroutine(AturdidoCoroutine(duracion));
    }
    */

}



/*
 * 
 * Para luz:
enemy.estaIluminado = true;

Para ruido:
enemy.HearNoise(transform.position, 20f);
*/