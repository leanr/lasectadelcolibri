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

    [Header("Movimiento")]
    //public float speed = 3f;
    private Rigidbody2D rb;
    private Transform target;
    public float baseSpeed = 3f;
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
    public float incrementPerSecond = 10f; // aumento por segundo
    public float decrementPerSecond = 5f;  // disminución por segundo
    public float maxSpeedBonus = 2f;      // cuánto aumenta la velocidad
    public float maxVisionBonus = 5f;     // cuánto aumenta la visión



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






    void Awake()
    {



        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        patrolCenter = transform.position;


        // Buscar player por tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;

        
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
        if (enemyType == EnemyType.SensibleALuz && estaIluminado)
        {
            
            //Agregar codigo de comportamiento de enemigo cuando lo iluminen
            attention += lightAttentionBonus * Time.fixedDeltaTime;
        }

        attention = Mathf.Clamp(attention, 0, attentionMax);



        // ======================
        // RUIDO TIENE PRIORIDAD
        // ======================
        if (enemyType == EnemyType.SensibleARuido && heardNoise)
        {

            //ACA VA LOGICA DE QUE HACE EL ENEMIGO CUANDO ESCUCHA RUIDO

            MoveTowards(noisePosition, baseSpeed);

            if (Vector2.Distance(transform.position, noisePosition) < 0.3f)
                heardNoise = false;

            return;
        }


        // ======================
        // INATURDIBLE
        // ======================
        if (enemyType == EnemyType.Inaturdible )
        {

            //Hacer algo
          
        }


        // ======================
        // Veloz
        // ======================
        if (enemyType == EnemyType.Veloz)
        {

            //Hacer algo
            //Aumentar velocidad

        }






        // Este es el movimiento cuando apareces en el rango de vision del enemigo



        if (distanceToPlayer <= baseVisionRange)
        {
            attention += incrementPerSecond * Time.fixedDeltaTime;
        }
        else
        {
            attention -= decrementPerSecond * Time.fixedDeltaTime;
        }

        attention = Mathf.Clamp(attention, 0f, attentionMax);

        // Valores dinámicos
        float visionRangeActual =
            baseVisionRange + (attention / attentionMax) * maxVisionBonus;

        float speedActual =
            baseSpeed + (attention / attentionMax) * maxSpeedBonus;

        // --- PERSECUCIÓN ---
        if (distanceToPlayer <= visionRangeActual)
        {
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