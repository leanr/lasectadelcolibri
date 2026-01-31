using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

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
    private float finAturdimiento = 0f;




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

    [Header("Detección Player")]
    public bool playerDetected = false;
    float detectRange = 5f;   // ver / perder
    float chaseRange = 8f;   // seguir

    bool damageBonus = false;



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
    public float visionRange = 5f;


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
            // rb.linearVelocity = Vector2.zero;
            // return;
            if (Time.time >= finAturdimiento)
            {
                aturdido = false;
                Debug.Log("Enemigo recuperado");
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
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
        /* if (enemyType == EnemyType.SensibleALuz && playerVision.torch.isOn)
         {

             //Agregar codigo de comportamiento de enemigo cuando lo iluminen
             //subo la atencion extra
             attention += lightAttentionBonus * Time.fixedDeltaTime;
             print("el Enemigo detecto la luz y aumento su daño");
             damageBonus = true;

         }
        */

        if (enemyType == EnemyType.SensibleALuz && IsPlayerInVisionRange() && playerVision.torch.isOn && !damageBonus)
        {
            attention += lightAttentionBonus * Time.fixedDeltaTime;
            
            Debug.Log($"{name} ve al jugador");
            Debug.Log($"{name} activo el damagebonus");
            damageBonus = true;
        }
        else
        {
           // damageBonus = false; desactivado!
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


       
        ///----------
        /// INATURDIBLE (solo modifica la dirección)
        ///----------
        if (enemyType == EnemyType.Inaturdible && directionToPlayer.sqrMagnitude > 0.001f)
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

                        // solo esquiva lo que está adelante
                        if (Vector2.Dot(pursueDir, toObstacle.normalized) > 0.3f)
                        {
                            float strength =
                                1f - Mathf.Clamp01(toObstacle.magnitude / avoidRadius);

                            evadeDir -= toObstacle.normalized * strength;
                        }
                    }
                }
            }

            // combinar persecución + evasión
            directionToPlayer = pursueDir + evadeDir;
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
        // if (distanceToPlayer <= visionRangeActual)
        // {


        //AGREGO LA ANIMACION de alerta o sonido de deteccion 
        bool canSeePlayer = distanceToPlayer <= detectRange;
        bool shouldChase = distanceToPlayer <= chaseRange;

        // 👉 ENTRADA AL RANGO DE VISIÓN
        if (canSeePlayer && !playerDetected)
            {
                playerDetected = true;
             

            // 🔊 Sonido de alerta
            //if (audioSource != null && detectPlayerSFX != null)
            //{
            //    audioSource.PlayOneShot(detectPlayerSFX);
            // }

            Debug.Log($"{gameObject.name} detectó al player");
            }

        // 👉 SALIDA DEL RANGO DE VISIÓN (opcional)
        if (!shouldChase && playerDetected)
        {
                playerDetected = false;
                print("salgo del area de deteccion del enemigo");
                print("desactivo el damage bonus");
                damageBonus = false;

         }

        if (!playerDetected)
        {
            damageBonus = false;
        }   

        ///---------------- termina zona de a




        if (playerDetected && distanceToPlayer <= chaseRange)
        {
            if (directionToPlayer.sqrMagnitude < 0.0001f)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (canSeePlayer)
            {
                Vector2 dir = directionToPlayer.normalized;
            rb.linearVelocity = dir * speedActual;

            hasPatrolTarget = false;
            }
        }
        // }

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

    bool IsPlayerInVisionRange()
    {
        float distance = Vector2.Distance(
            transform.position,
            target.transform.position
        );

        if (distance > visionRange)
            return false;

        return true;
    }

    public float damageOnHit = 10f;
    public float damageCooldown = 1f; // segundos entre daños
    private float nextDamageTime = 0f;




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time < nextDamageTime)
                return;


            PlayerController player =
                collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                //player.TakeDamage(damageOnHit);
                print("el player recibe daño");
                if (damageBonus == true)
                {
                    player.currentHealth = player.currentHealth -  20;
                    print("hice 20 de daño");
                }
                else
                {
                    player.currentHealth = player.currentHealth - 10;
                    print("hice 10 de daño");
                }

                //dejo listo para el efecto de audio TODO: efectoGolpe
                //if (audioSource != null && hitPlayerSFX != null)
                //{
                //    audioSource.PlayOneShot(hitPlayerSFX);
                //}



                nextDamageTime = Time.time + damageCooldown;
            }
        }


        if (collision.transform.root.CompareTag("Stunner"))
        {
            //el enemigo se aturde
            print("el enemigo esta aturdido y no puede moverse");
            aturdido = true;
            finAturdimiento = Time.time + tiempoAturdido;


        }

    }

        
    }

